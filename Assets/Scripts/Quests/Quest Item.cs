using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    public int questNumber;
    public string itemName;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            if(!QuestManager.Instance.questCompleted[questNumber] && QuestManager.Instance.quests[questNumber].gameObject.activeSelf) {
                QuestManager.Instance.itemCollected = itemName;
                gameObject.SetActive(false);
            }
        }
    }
}
