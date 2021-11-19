using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource playerDead;
    public AudioSource levelUP;
    public AudioSource itemPickedUp;
    public AudioSource itemThrow;
    public AudioSource breakCrate;

    public AudioSource soundTrack;

    public AudioSource dash;

    public AudioSource leashAttach;
    public AudioSource leashDetach;

    public AudioSource owl;
    public AudioSource birds;
    public AudioSource crickets;
    public AudioSource cricketsFar;
    public AudioSource parkNightAtmosphere;
    public AudioSource sniff;

    public AudioSource[] footsteps;

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
        //soundTrack.Stop();
        PlayOnLoop(source);
    }

    private void PlayRandomFootstep() {
        int random = Random.Range(0, footsteps.Length);
        PlaySound(footsteps[random]);
    }
}
