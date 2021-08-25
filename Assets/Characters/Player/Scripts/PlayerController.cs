using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    Transform transform;
    [SerializeField] float moveSpeed;
    Animator animator;
    int walkingSide = 0;

    int health = 5;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {
            walkingSide = 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            walkingSide = 2;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            walkingSide = 1;
        }

        rigidbody2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        animator.SetFloat( "Velocity", Mathf.Abs(rigidbody2d.velocity.x + rigidbody2d.velocity.y) );
        animator.SetInteger( "WalkingSide", walkingSide);

        
    }

    public void Hit(int amount)
    {
        this.health -= amount;
        Debug.Log("Crap, I have only " + health + " left!");
    }
}