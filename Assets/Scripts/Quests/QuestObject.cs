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
    public bool isBoneQuest;

    public bool isItemQuest;
    public string targetItem;

    private bool milicaDialogueStarted;

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
                GameObject.Find("VladoScentBurst").GetComponent<ParticleSystem>().Play();
            };
            if (QuestManager.Instance.sniffButtonClicked == true && QuestManager.Instance.itemCollected == targetItem) {
                QuestManager.Instance.itemCollected = null;

                GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList(1);

                EndQuest();
                QuestManager.Instance.quests[1].gameObject.SetActive(true);
                QuestManager.Instance.quests[1].StartQuest();
            }
        }
        if (isMilicaQuest) {
            if (milicaDialogueStarted) {
                if (DialogueManager.Instance.animator.GetBool("IsOpen") == false) {
                    GameObject.Find("Finger").GetComponent<Animator>().SetBool("isShowing", true);
                    Leash.Instance.canPutLeashOnOrOff = true;
                    milicaDialogueStarted = false;
                }
            }
        }
        if (isItemQuest) {
            if(QuestManager.Instance.itemCollected == targetItem) {
                QuestManager.Instance.itemCollected = null;

                EndQuest();
            }
        }
        if (isBoneQuest) {
            if (QuestManager.Instance.quests[2].gameObject.activeSelf) {
                QuestManager.Instance.quests[1].EndQuest();
                GameObject.Find("ToDo List Panel").GetComponent<TaskManager>().CheckTaskOnToDoList(2);
            }
        }
    }

    public void StartQuest() {
        if (isSmellQuest) {
            GameObject.Find("ScentButton").GetComponent<Animator>().SetBool("pingPong", true);
        }
        if (isMilicaQuest) {
            StartCoroutine(MilicaDialogue());
        } else if (isBoneQuest) {
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
        GameObject.Find("ToDo List Panel").GetComponent<Animator>().SetBool("isShowing", false);
        yield return new WaitForSeconds(2f);
        QuestManager.Instance.ShowQuestText(startDialogue);
        milicaDialogueStarted = true;
    }
}
