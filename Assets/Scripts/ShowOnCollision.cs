using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ShowOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GetComponent<ParticleSystem>().Play();
            SFXManager.Instance.PlaySound(SFXManager.Instance.sniff);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GetComponent<ParticleSystem>().Stop();
            SFXManager.Instance.StopSound(SFXManager.Instance.sniff);
        }
    }
}
