using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Text textArea;
    Animator animator;
    [TextArea(2, 5)] public string[] texts;
    public Queue<string> sentences;

    [HideInInspector] public bool dialoguing = false;

    bool lockPlayer = false;

    void Start()
    {
        sentences = new Queue<string>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!dialoguing)
                animator.SetTrigger("Pop In");
            else
                DisplayNextSentence();
        }
    }

    void StartDialogue()
    {
        sentences.Clear();
        dialoguing = true;
        GameObject.Find("Player").GetComponent<PlayerController>().locked = true;

        foreach(string sentence in texts)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        textArea.text = sentence;  
    }

    public void EndDialogue()
    {
        dialoguing = false;
        textArea.text = "";

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Pop Out"))
            animator.SetTrigger("Pop Out");
        
        sentences.Clear();
        GameObject.Find("Player").GetComponent<PlayerController>().locked = false;
    }
}