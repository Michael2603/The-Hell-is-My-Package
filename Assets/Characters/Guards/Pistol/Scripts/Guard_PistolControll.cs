using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard_PistolControll : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Collider2D collider2d;
    Transform transform;
    float detectionRate = 0;
    string patrolType = "Idle";
    string persuitType = "Focused";

    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        DetectPlayer();
        if (detectionRate >= 1)
        {
            PlayerFocused();
        }
        else
        {
            Patrol();
        }
    }

    //Controls the detection mechanics
    void DetectPlayer()
    {
        slider.value = detectionRate;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (detectionRate > .6f)
        {
            Debug.Log("I think I saw something strange...");
        }
    }

    // Controls the 2 states of patrol (The first is the guard idle but with a high range track and the secound is the guard walking through a route but with a reduced sight)
    void Patrol()
    {
        switch(patrolType)
        {
            case "Idle":
                Collider2D checkZone = Physics2D.OverlapCircle(transform.position, 10, 1 << LayerMask.NameToLayer("Player"));
                if ( checkZone && detectionRate < 1)
                {
                    detectionRate += Time.deltaTime * .8f;
                }
                else if ( !checkZone && detectionRate > 0 )
                {
                    detectionRate -= Time.deltaTime * .1f;
                }

            break;
            case "Route":

            break;
        }
    }

    // Controls the 2 states of persuit (when he focus and attack the player and when the player leaves his sight and he starts to search for the player)
    void PlayerFocused()
    {
        switch (persuitType)
        {
            case "Focused":
                Collider2D playerFound = Physics2D.OverlapCircle(transform.position, 10, 1 << LayerMask.NameToLayer("Player"));

                if (playerFound)
                {
                    
                }

            break;
            case "Searching":

            break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 10);
    }
}
