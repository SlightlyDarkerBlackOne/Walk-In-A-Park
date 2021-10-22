using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource levelUP;
    public AudioSource itemPickedUp;
    public AudioSource breakCrate;

    public AudioSource soundTrack;

    public AudioSource dash;
    public AudioSource footsteps;

    public AudioSource dogBark;
    public AudioSource dogCry;

    #region Singleton
    public static SFXManager Instance { get; private set; }

    // Use this for initialization
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    #endregion

    public void PlaySound(AudioSource source) {
        source.Play();
    }
    private void PlayOnLoop(AudioSource source) {
        source.Play();
        source.loop = true;
    }

    public void PlaySoundTrack(AudioSource source) {
        soundTrack.Stop();
        PlayOnLoop(source);
    }
}
