using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    Rigidbody2D rigidbody2d;
    Transform transform;
    [SerializeField] float moveSpeed;
    Animator animator;
    int health = 5;

    public bool canReceiveInput;
    public bool inputReceived;

    [SerializeField] Collider2D atk1;
    [SerializeField] Collider2D atk2;
    [SerializeField] Collider2D atk3;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
 
        if (rigidbody2d.velocity.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }
        else if (rigidbody2d.velocity.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }

        animator.SetFloat("Move X", Input.GetAxis("Horizontal"));
        animator.SetFloat("Move Y", Input.GetAxis("Vertical"));

        MovementSystem();
    }

    void MovementSystem()
    {
        rigidbody2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);

        if (Input.GetMouseButtonDown(0))
        {
            if ( canReceiveInput )
            {
                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }

    }

    public void Hit(int amount)
    {
        this.health -= amount;
        Debug.Log("Crap, I have only " + health + " left!");
    }

    public void InputManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
}