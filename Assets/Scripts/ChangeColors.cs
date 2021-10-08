using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeColors : MonoBehaviour
{
    Tilemap[] tilemaps;
    private Color color;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        tilemaps = GetComponentsInChildren<Tilemap>();

        if (ColorUtility.TryParseHtmlString("#FDCFCF", out color)){}
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out originalColor)) { }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController2D.Instance.gameObject.transform.Find("Leash").gameObject.activeSelf) {
            foreach (Tilemap tilemap in tilemaps) {
                tilemap.color = color;
            }
        } else {
            foreach (Tilemap tilemap in tilemaps) {
                tilemap.color = originalColor;
            }
        }
    }
}
