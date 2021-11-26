using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSound : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Debug.Log("owl");
            SFXManager.Instance.PlaySound(SFXManager.Instance.owl);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.V)) {
            SFXManager.Instance.PlaySound(SFXManager.Instance.owl);
        }
    }
}
