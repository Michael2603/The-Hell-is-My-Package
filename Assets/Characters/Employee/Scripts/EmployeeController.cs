using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    Transform transform;
    Animator animator;
    [SerializeField] float moveSpeed;
    
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
        TrackBox();
    }

    void TrackBox()
    {
        Collider2D checkZone = Physics2D.OverlapCircle(rigidbody2d.position, 10, 1 << LayerMask.NameToLayer("Box"));
        if (checkZone != null)
            Debug.Log("ACHEI UMA CAAAAXA PO");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, 10);
    }       
}
