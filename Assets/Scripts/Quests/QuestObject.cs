using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using TMPro;

public class QuestObject : MonoBehaviour
{
    public int questNumber;

    public Dialogue startDialogue;
    public Dialogue endDialogue;

    public bool isSmellQuest;
    public bool isMilicaQuest;

    public bool isItemQuest;
    public string targetItem;

    // Start is called before the first frame update
    void Start()
    {
        endDialogue.name = startDialogue.name;
    }

    private void Update() {
        if (isSmellQuest) {
            GameObject.Find("ScentButton").GetComponent<Button_UI>().ClickFunc = () => {
                QuestManager.Instance.sniffButtonClicked = true;

                GameObject.Find("ScentButton").GetComponent<Animator>().SetBool("pingPong", false);
                GameObject.Find("VladoScent").GetComponent<ParticleSystem>().Play();
            };
            if (QuestManager.Instance.sniffButtonClicked == true && QuestManager.Instance.itemCollected == targetItem) {
                QuestManager.Instance.itemCollected = null;

                GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList();

                EndQuest();
                QuestManager.Instance.quests[1].gameObject.SetActive(true);
                QuestManager.Instance.quests[1].StartQuest();
            }
        }
        if (isMilicaQuest) {
            
        }
        if (isItemQuest) {
            if(QuestManager.Instance.itemCollected == targetItem) {
                QuestManager.Instance.itemCollected = null;

                EndQuest();
            }
        }
    }

    public void StartQuest() {
        if (isSmellQuest) {
            GameObject.Find("ScentButton").GetComponent<Animator>().SetBool("pingPong", true);
        }
        if (isMilicaQuest) {
            StartCoroutine(MilicaDialogue());
        } else {
            QuestManager.Instance.ShowQuestText(startDialogue);
        }
    }

    public void EndQuest() {
        QuestManager.Instance.ShowQuestText(endDialogue);
        QuestManager.Instance.questCompleted[questNumber] = true;
        gameObject.SetActive(false);
    }

    IEnumerator MilicaDialogue() {
        yield return new WaitForSeconds(4f);
        QuestManager.Instance.ShowQuestText(startDialogue);
        yield return new WaitForSeconds(3f);
        //if (DialogueManager.Instance.animator.GetBool("IsOpen") == false) {
            GameObject.Find("Finger").GetComponent<Animator>().SetBool("isShowing", true);
        GameObject.Find("ToDo List Panel").GetComponent<Animator>().SetBool("isShowing", false);
        //}
    }
}
