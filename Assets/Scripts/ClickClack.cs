using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClickClack : MonoBehaviour
{
    private Animator anim;
    private Transform clickClackPosition;
    private bool canClickClack;
    private bool clickClacking;
    private bool insideTrigger;
    private bool enteredTriggerArea = false;
    private void Start() {
        anim = GetComponent<Animator>();
        clickClackPosition = transform.Find("PlayerLocation");
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) || Tap()) {
            if (canClickClack && !GameManager.Instance.leashActive) {
                anim.SetBool("isClickClacking", true);
                PlayerController2D.Instance.SetPlayerToLocationAndFreeze(clickClackPosition);
                clickClacking = true;
                canClickClack = false;

                UIManager.Instance.ShowStopClickClackText();
                ChangeSortingOrderSoPlayerIsOnTop(); 
            } else if(!canClickClack && insideTrigger){
                PlayerController2D.Instance.RemovePlayerFromLocationAndUnfreeze();
                clickClacking = false;
            }
        }
        if (clickClacking) {
            PlayerController2D.Instance.SetPlayerToLocation(clickClackPosition);
        }
    }

    private void OnTriggerEnter2D()
    {
        enteredTriggerArea = true;
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            insideTrigger = true;
            if (!clickClacking) {
                UIManager.Instance.ShowClickClackText();
                canClickClack = true;
                anim.SetBool("isClickClacking", false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            insideTrigger = false;
            UIManager.Instance.HideClickClackIndicatorText();
            canClickClack = false;
        }
    }
    //It removes script and sets sorting order just for a moment
    //This could cause bad performance later
    private void ChangeSortingOrderSoPlayerIsOnTop() {
        Destroy(gameObject.GetComponent<SortByYAxis>());
        GetComponent<SpriteRenderer>().sortingOrder = -200;
    }

    private bool Tap()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (enteredTriggerArea)
            {
                enteredTriggerArea = false;
                return false;
            }
            else
            {
                return true;
            }        
        }
        return false;
    }
}
