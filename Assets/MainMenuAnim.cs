using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAnim : MonoBehaviour
{//frames
    [SerializeField]
    Sprite[] Frames;
 
    // speed
    [SerializeField]
    float framesPerSecond = 10f;

    private Image im;

    void Start() {
        im = this.gameObject.GetComponent<Image>();
        
    }
 
    void Update()
    {
        // get index of frame
        int index = (int)(Time.time * framesPerSecond) % Frames.Length;
        // check if the Texture array don't equal null
        if (Frames[index] != null)
        {
            // get Renderer of this gameobject
            im.overrideSprite = Frames[index];
        }
    }

}
