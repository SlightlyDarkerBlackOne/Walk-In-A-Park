using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestObject[] quests;
    public bool[] questCompleted;

    [HideInInspector]
    public string itemCollected;

    [HideInInspector]
    public string enemyKilled = "";
    
    #region Singleton
    public static QuestManager Instance {get; private set;}
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start() {
        questCompleted = new bool[quests.Length];
    }

    public void ShowQuestText(Dialogue questDialogue) {
        DialogueManager.Instance.StartDialogue(questDialogue);
    }

    /*public void TriggerQuestDialogue(Dialogue questDialogue) {
        theDM.StartDialogue(questDialogue);
    }*/
}
