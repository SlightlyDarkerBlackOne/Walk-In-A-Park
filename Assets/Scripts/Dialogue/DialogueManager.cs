﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour {

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image avatarImage;

    public Animator animator;

    private Queue<string> sentences;

    private bool shouldBark;

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
    public void StartDialogue(Dialogue dialogue, Sprite avatar) {
        avatarImage.sprite = avatar;
        StartDialogueHere(dialogue);
    }
    public void StartDialogue(Dialogue dialogue) {
        StartDialogueHere(dialogue);
    }

    private void StartDialogueHere(Dialogue dialogue) {
        PlayerController2D.Instance.FrezePlayer();
        avatarImage.sprite = dialogue.avatar;
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences = new Queue<string>();
        if (sentences.Count != 0)
            sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        if(dialogue.name == "Bosko") {
            shouldBark = true;
        } else {
            shouldBark = false;
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
        if (shouldBark) {
            SFXManager.Instance.PlayRandomBark();
        }
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

    public void EndDialogue() {
        animator.SetBool("IsOpen", false);
        PlayerController2D.Instance.UnFreezePlayer();
    }
}
