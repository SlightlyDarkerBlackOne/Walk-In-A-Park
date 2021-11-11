using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PeeIcon : MonoBehaviour
{
    private Button_UI button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button_UI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        button.GetComponent<Button_UI>().ClickFunc = () => {
            Pissing.ClickedPeeIcon = true;
        };
        
    }
}
