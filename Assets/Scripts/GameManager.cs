using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SFXManager.Instance.PlaySound(SFXManager.Instance.soundTrack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
