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

    public bool isItemQuest;
    public string targetItem;

    public bool isEnemyQuest;
    public string targetEnemy;
    public int enemiesToKill;
    public int enemyKillCount;

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
            }
        }
        if (isItemQuest) {
            if(QuestManager.Instance.itemCollected == targetItem) {
                QuestManager.Instance.itemCollected = null;

                EndQuest();
            }
        }

        if (isEnemyQuest) {
            if(QuestManager.Instance.enemyKilled == targetEnemy) {
                QuestManager.Instance.enemyKilled = "";

                enemyKillCount++;
            }

            if(enemyKillCount >= enemiesToKill) {
                EndQuest();
            }
        }
    }

    public void StartQuest() {
        QuestManager.Instance.ShowQuestText(startDialogue);
        if (isSmellQuest) {
            GameObject.Find("ScentButton").GetComponent<Animator>().SetBool("pingPong", true);
        }
    }

    public void EndQuest() {
        QuestManager.Instance.ShowQuestText(endDialogue);
        QuestManager.Instance.questCompleted[questNumber] = true;
        gameObject.SetActive(false);
    }
}
