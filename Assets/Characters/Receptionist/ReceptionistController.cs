using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionistController : MonoBehaviour
{
    public GameObject exclamation;

    bool canTalk = true;
    bool next_Trigger = false;
    float nextTextTimer = 1;

    DialogueManager dm;

    void Start()
    {
        dm = transform.GetChild(0).GetComponent<DialogueManager>();
    }
    
    void Update()
    {
        if (!next_Trigger)
        {return;}


        if (!dm.dialoguing)
        {
            dm.gameObject.GetComponent<Animator>().SetTrigger("Pop In");
        }
        else
        {
            dm.DisplayNextSentence();
        }

        canTalk = false;
        next_Trigger = false;
        
    }

    void FixedUpdate()
    {
        if (canTalk)
        {return;}

        nextTextTimer -= Time.deltaTime;

        if (nextTextTimer <= 0)
        {
            nextTextTimer = 1;
            canTalk = true;
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetMouseButtonDown(1) && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            exclamation.SetActive(false);
            next_Trigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        dm.EndDialogue();
        next_Trigger = false;
        canTalk = true;
    }
}
