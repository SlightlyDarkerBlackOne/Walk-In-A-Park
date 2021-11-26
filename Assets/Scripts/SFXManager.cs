using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource playerDead;
    public AudioSource levelUP;
    public AudioSource itemPickedUp;
    public AudioSource itemThrow;

    public AudioSource soundTrack;
    public AudioSource reward;

    public AudioSource dash;

    public AudioSource leashAttach;
    public AudioSource leashDetach;

    public AudioSource owl;
    public AudioSource birds;
    public AudioSource crickets;
    public AudioSource cricketsFar;
    public AudioSource parkDayAtmosphere;
    public AudioSource parkNightAtmosphere;
    public AudioSource sniff;

    public AudioSource[] footsteps;
    public AudioSource[] uiButtons;
    public AudioSource[] barks;
    public AudioSource[] noteCross;


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
    public void StopSound(AudioSource source) {
        source.Stop();
    }
    private void PlayOnLoop(AudioSource source) {
        source.Play();
        source.loop = true;
    }

    public void PlaySoundTrack(AudioSource source) {
        //soundTrack.Stop();
        PlayOnLoop(source);
    }
    public void PlayAtmosphere() {
        if (parkDayAtmosphere.isPlaying) {
            PlayOnLoop(parkNightAtmosphere);
            parkDayAtmosphere.Stop();
        } else if (parkNightAtmosphere.isPlaying) {
            PlayOnLoop(parkDayAtmosphere);
            parkNightAtmosphere.Stop();
        }
    }

    public void PlayRandomFootstep() {
        int random = Random.Range(0, footsteps.Length);
        PlaySound(footsteps[random]);
    }
    public void PlayRandomBark() {
        int random = Random.Range(0, barks.Length);
        PlaySound(barks[random]);
    }
    public void PlayNoteCrossing() {
        PlaySound(noteCross[0]);
        StartCoroutine(Wait());
        PlaySound(noteCross[1]);
    }
    
    IEnumerator Wait() {
        yield return new WaitForSeconds(0.1f);
    }
}
