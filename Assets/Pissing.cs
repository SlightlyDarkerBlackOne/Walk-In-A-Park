using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pissing : MonoBehaviour
{
    [SerializeField]
    private GameObject pissParticle;
    [SerializeField]
    private GameObject pissStain;
    GameObject piss;
    [SerializeField]
    private float pissCD;

    public static bool ClickedPeeIcon = false;
    private bool pissing = false;

    void Update()
    {
        if (!pissing && (Input.GetKeyDown(KeyCode.F) || ClickedPeeIcon)) {
            StartCoroutine(PissingCO());
            if (ClickedPeeIcon) ClickedPeeIcon = false;
        }
        if (piss) {
            piss.transform.position = transform.position;
        }
    }

    private IEnumerator PissingCO() {
        pissing = true;
        piss = Instantiate(pissParticle, Vector2.zero, Quaternion.Euler(0, 0, -118f)) as GameObject;
        piss.transform.position = transform.position;
        PlayerController2D.Instance.FrezePlayer();
        yield return new WaitForSeconds(pissCD);
        GameObject pissStainGO = Instantiate(pissStain, new Vector2(transform.position.x - 1.4f, transform.position.y - 0.4f), Quaternion.identity) as GameObject;
        pissStainGO.AddComponent<SortByYAxis>();
        PlayerController2D.Instance.UnFreezePlayer();
        Destroy(piss);
        pissing = false;
    }
}
