using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    Transform transform;
    [SerializeField] float moveSpeed;

    int health = 5;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        rigidbody2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
    }

    public void Hit(int amount)
    {
        this.health -= amount;
        Debug.Log("Crap, I have only " + health + " left!");
    }
}