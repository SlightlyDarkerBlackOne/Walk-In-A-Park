using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Sprite humanImage;
    public Sprite dogImage;

    private Sprite spriteToSet;
    // Start is called before the first frame update
    void Start()
    {
        humanImage = transform.Find("PlayerImage").gameObject.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController2D.Instance.gameObject.transform.Find("Leash").gameObject.activeSelf) {
            transform.Find("PlayerImage").gameObject.GetComponent<Image>().sprite = dogImage;
        } else {
            transform.Find("PlayerImage").gameObject.GetComponent<Image>().sprite = humanImage;
        }
    }
}
