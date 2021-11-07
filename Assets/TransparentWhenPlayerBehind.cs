using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWhenPlayerBehind : MonoBehaviour
{
    [Range(0, 1)]
    public float alpha;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            ChangeAlpha(alpha);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        ChangeAlpha(1f);
    }

    public void ChangeAlpha(float alphaValue) {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alphaValue);
    }
}
