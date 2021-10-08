using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        private enum State
        {
            Move,
            Follow
        }

        private State state;

        public float speed;
        public float moveSpeedChaseModifier = 0.76f;
        public float moveSpeedDropMultiplier = 8;
        public float moveSpeedDropOffset = 0.2f;

        public float minDistance = 1.5f;

        public GameObject leash;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            if (leash.activeSelf) {
                state = State.Move;
            } else {
                state = State.Follow;
            }
        }


        private void Update()
        {
            Move();            
        }

        private void Move()
        {
            Vector2 dir = Vector2.zero;
            switch (state) {
                case State.Move:
                    if (!leash.activeSelf) {
                        state = State.Follow;
                    }
                    if (Input.GetKey(KeyCode.A)) {
                    dir.x = -1;
                    animator.SetInteger("Direction", 3);
                    } else if (Input.GetKey(KeyCode.D)) {
                        dir.x = 1;
                        animator.SetInteger("Direction", 2);
                    }

                    if (Input.GetKey(KeyCode.W)) {
                        dir.y = 1;
                        animator.SetInteger("Direction", 1);
                    } else if (Input.GetKey(KeyCode.S)) {
                        dir.y = -1;
                        animator.SetInteger("Direction", 0);
                    }
                    break;
                case State.Follow:
                    if (leash.activeSelf) {
                        state = State.Move;
                    }
                    Follow();
                    break;
            }
            
            dir.Normalize();

            animator.SetBool("IsMoving", dir.magnitude > 0);
            GetComponent<Rigidbody2D>().velocity = speed * dir;
        }
        private void Follow() {
            float distanceBetweenPlayers = Vector2.Distance(transform.position, PlayerController2D.Instance.transform.position);
            //Lerp moveSpeedChaseModifier 0.2 - 1 based on Distance
            float lerpedDistance = Mathf.Lerp(0f, 1f, distanceBetweenPlayers/moveSpeedDropMultiplier) - moveSpeedDropOffset;
            moveSpeedChaseModifier = Mathf.Lerp(0.1f, 1.5f, lerpedDistance);

            if (distanceBetweenPlayers >= minDistance) {
                transform.position = Vector2.MoveTowards(transform.position, PlayerController2D.Instance.transform.position, speed * moveSpeedChaseModifier * Time.deltaTime);
                animator.SetBool("IsMoving", true);
            }
        }
    }
}
