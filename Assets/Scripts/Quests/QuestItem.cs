using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            if(itemName == "PinkBall") {
                GameObject.Find("Dog Cookie").GetComponent<Animator>().SetTrigger("getCookie");
                GameObject.Find("Dog Cookie").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
            }
        }
    }
}
