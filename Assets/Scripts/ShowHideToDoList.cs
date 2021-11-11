using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class ShowHideToDoList : MonoBehaviour
{
    private Button_UI button;
    [SerializeField]
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button_UI>();
    }

    // Update is called once per frame
    void Update() {
        button.GetComponent<Button_UI>().ClickFunc = () => {
            if (anim.GetBool("isShowing")) {
                anim.SetBool("isShowing", false);
            } else {
                anim.SetBool("isShowing", true);
            }
        };
    }

}
