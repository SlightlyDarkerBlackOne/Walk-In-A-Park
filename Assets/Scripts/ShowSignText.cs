using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowSignText : MonoBehaviour
{
    public GameObject text;
    private bool alreadyShown = false;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if(!alreadyShown)
                ShowText();   
        }
    }

    private void ShowText() {
        text.SetActive(true);
        text.GetComponent<Animator>().SetTrigger("show");
        alreadyShown = true;
    }
}
