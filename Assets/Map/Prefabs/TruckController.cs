using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Transform transform;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Depart();
        }
    }

    public void Depart()
    {
        rigidbody2d.AddForce(transform.up * -200);
    }
}