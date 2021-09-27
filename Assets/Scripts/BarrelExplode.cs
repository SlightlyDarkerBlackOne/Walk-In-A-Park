using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelExplode : MonoBehaviour
{
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            anim.SetTrigger("Explode");
            SFXManager.Instance.PlaySound(SFXManager.Instance.breakCrate);
            foreach (Collider2D collider in this.GetComponents<CircleCollider2D>()) {
                collider.enabled = false;
            }
            Destroy(gameObject, 2f);
        }
    }
}
