using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationWithDelay : MonoBehaviour
{
    private Animator anim;
    void Start() {
        anim = GetComponent<Animator>();
        StartCoroutine(PlayAnimAfterRandomTime());
    }

    private IEnumerator PlayAnimAfterRandomTime() {
        float randomDelay = Random.Range(2f, 12f);
        yield return new WaitForSeconds(randomDelay);
        anim.SetTrigger("Play");
    }
}
