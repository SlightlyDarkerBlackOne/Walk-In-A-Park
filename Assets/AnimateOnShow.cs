using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateOnShow : MonoBehaviour
{
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        anim.SetBool("Show", true);
    }

    public void StopAnimating() {
        anim.SetBool("Show", false);
    }
}
