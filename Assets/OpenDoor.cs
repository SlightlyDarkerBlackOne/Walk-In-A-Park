using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !PlayerController2D.Instance.gameObject.transform.Find("Leash").gameObject.activeSelf) {
            Destroy(gameObject);
        }
    }
}
