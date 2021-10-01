using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    #region Singleton
    public static DialogueManager Instance {get; private set;}
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
	// Use this for initialization
	void Start () {
        sentences = new Queue<string>();
	}
    
    //For starting a dialogue and also for starting quest dialogue
    public void StartDialogue(Dialogue dialogue) {
        PlayerController2D.Instance.FrezePlayer();
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences = new Queue<string>();
        if(sentences.Count != 0)
            sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();

        //Stops animating last sentence if we start with the new one (continue button)
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //Writes letter by letter in dialogue
    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        animator.SetBool("IsOpen", false);
        PlayerController2D.Instance.UnFreezePlayer();
    }
}
