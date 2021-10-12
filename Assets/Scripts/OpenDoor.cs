using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !PlayerController2D.Instance.gameObject.transform.Find("Leash").gameObject.activeSelf) {
            Destroy(gameObject);
            SFXManager.Instance.PlaySound(SFXManager.Instance.breakCrate);
        }
    }
}
