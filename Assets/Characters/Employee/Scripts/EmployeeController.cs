using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    Transform transform;
    Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] string state = "TrackingPackage";
    Transform boxPos;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody2d.velocity.x) > 0 || Mathf.Abs(rigidbody2d.velocity.y) > 0)
            animator.SetFloat("Velocity", 1);
        
        switch (state)
        {
            case "TrackingPackage":
                TrackBox();
            break;
            case "PackingBox":
                PackingBox();
            break;
        }
    }

    void TrackBox()
    {
        Collider2D checkZone = Physics2D.OverlapCircle(rigidbody2d.position, 10, 1 << LayerMask.NameToLayer("Box"));
        if (checkZone != null)
        {
            state = "PackingBox";
            boxPos = checkZone.gameObject.GetComponent<Transform>();
            Debug.Log(boxPos);
        }else
        {
            boxPos = null;
        }
    }

    void PackingBox()
    {
        Vector2.MoveTowards(transform.position, boxPos.position, moveSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, 10);
    }       
}
