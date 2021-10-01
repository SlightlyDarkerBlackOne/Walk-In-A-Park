using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public int questNumber;

    public Dialogue startDialogue;
    public Dialogue endDialogue;

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
    }

    public void EndQuest() {
        QuestManager.Instance.ShowQuestText(endDialogue);
        QuestManager.Instance.questCompleted[questNumber] = true;
        gameObject.SetActive(false);
    }
}
