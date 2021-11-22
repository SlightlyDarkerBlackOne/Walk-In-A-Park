using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DialogueTrigger : MonoBehaviour {

    public bool showOnce;
    public bool story;
    public bool destroy;
    public bool showOnClick;
    private bool shownAllready = false;
    public Animator anim;

    public Dialogue dialogue;
    public Sprite avatar;

    private void Start(){
        if (story){
            TriggerDialogue();
        }
    }
    public void TriggerDialogue(){
        DialogueManager.Instance.StartDialogue(dialogue, avatar);
    }

    //When entering dialogue or NPC dialogue zone
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
            if (anim != null) {
                anim.SetBool("Show", true); //Dialogue indicator
            }
            if (!shownAllready && !showOnClick) {
                TriggerDialogue();
                if (showOnce) {
                    shownAllready = true;
                    if (destroy)
                        Destroy(gameObject);
                }
            }
            //Hide To Do List
            GameObject.Find("ToDo List Panel").GetComponent<Animator>().SetBool("isShowing", false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            if (!showOnClick) {
            DialogueManager.Instance.EndDialogue();
            }

            if (anim != null) {
                anim.SetBool("Show", false);
            }
        }
    }
}
