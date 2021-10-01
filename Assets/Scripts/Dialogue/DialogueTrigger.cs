using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public bool showOnce;
    public bool story;
    public bool destroy;
    private bool shownAllready = false;

    public Dialogue dialogue;

    private void Start(){
        if (story){
            TriggerDialogue();
        }
    }
    public void TriggerDialogue(){
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    //When entering dialogue or NPC dialogue zone
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Player") {
            if(!shownAllready)
                TriggerDialogue();
            if (showOnce)
            {
                shownAllready = true;
                if(destroy)
                    Destroy(gameObject);
            }
        }
    }
}
