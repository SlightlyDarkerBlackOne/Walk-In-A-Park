using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayLevel : MonoBehaviour
{
    public GameObject playButton;

    public void PlayButtonAnimation() {
        playButton.GetComponent<Animator>().SetTrigger("Play");
    }
}
