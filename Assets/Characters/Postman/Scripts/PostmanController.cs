using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PostmanController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rigidbody2d;
    Collider2D coll;
    Transform transform;
    Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] string state;
    Transform box;
    GameObject objective;

    GameObject[] deliveryPoints;
    GameObject[] waitingPoints;

    public Transform target;
    public float nextWayPoinDistance = 3f;
    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        seeker = GetComponent<Seeker>();

        waitingPoints = GameObject.FindGameObjectsWithTag("WaitingPoint");
        deliveryPoints = GameObject.FindGameObjectsWithTag("DeliveryPoint");
        target = waitingPoints[Random.Range(0, waitingPoints.Length)].gameObject.GetComponent<Transform>();

       InvokeRepeating("UpdatePath", 0, .5f);
        
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody2d.velocity.x) > 0 || Mathf.Abs(rigidbody2d.velocity.y) > 0)
            animator.SetFloat("Velocity", 1);
        
        switch (state)
        {
            case "TrackPackage":
                TrackPackage();
            break;
            case "PickPackage":
                PickPackage();
            break;
            case "DeliveryPackage":
                DeliveryPackage();
            break;
        }
    }

    void TrackPackage()
    {
        Collider2D checkZone = Physics2D.OverlapCircle(rigidbody2d.position, 10, 1 << LayerMask.NameToLayer("Box"));
        if (checkZone != null)
        {
            box = checkZone.gameObject.GetComponent<Transform>();
                    
            target = box;
        }
        else
            box = null;

        GoAfterTarget();
    } 

    void PickPackage()
    {
        GoAfterTarget();

        // if (reachedEndOfPath)
        // {
        //     target = deliveryPoints[Random.Range(0, deliveryPoints.Length)].GetComponent<Transform>();
        //     state = "DeliveryPackage";
        // }

        // // Get the angle that the postman is pointing to by moving, using it, adds 2 check angles, one 45° upper and other 45° lower. These are used to check for walls and evade them
        // Vector3 movement =  new Vector3(rigidbody2d.velocity.x, rigidbody2d.velocity.y, 0);
        // float pointingAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

        // Vector3 upperCast = Quaternion.AngleAxis(pointingAngle + 45 , Vector3.forward) * Vector3.right * 20;
        // Vector3 lowerCast = Quaternion.AngleAxis(pointingAngle - 45, Vector3.forward) * Vector3.right * 20;

    }

    void DeliveryPackage()
    {
        GoAfterTarget();

        if (reachedEndOfPath)
            state = "TrackPackage";
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Check if is looking for a box and if has collided to a box, if so, grab it
        if ( state == "TrackPackage" && (other.gameObject.layer == LayerMask.NameToLayer("Box")) )
        {
            Transform boxTransform = box.gameObject.GetComponent<Transform>();
            Collider2D boxCollider = box.gameObject.GetComponent<Collider2D>();
            Rigidbody2D boxRb = box.gameObject.GetComponent<Rigidbody2D>();

            boxTransform.SetParent(this.transform);
            boxTransform.localPosition = new Vector3(.04f, 0, 0);
            boxCollider.isTrigger = true;
            boxRb.isKinematic = true;

            N ta indo entregar a caixa
            
            state = "DeliveryPackage";
            target = deliveryPoints[Random.Range(0, deliveryPoints.Length)].GetComponent<Transform>();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, 10);

        Vector3 movement =  new Vector3(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y, 0);
        float pointingAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

        Vector3 upperCast = Quaternion.AngleAxis(pointingAngle + 45 , Vector3.forward) * Vector3.right * 10;
        Vector3 lowerCast = Quaternion.AngleAxis(pointingAngle - 45, Vector3.forward) * Vector3.right * 10; 

        Gizmos.DrawRay(GetComponent<Transform>().position, upperCast);
        Gizmos.DrawRay(GetComponent<Transform>().position, lowerCast);
    }      

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rigidbody2d.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
            path = p;
            currentWayPoint = 0;
    }

    void GoAfterTarget()
    {
        if (path == null)
            return;
        
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ( (Vector2)path.vectorPath[currentWayPoint] - rigidbody2d.position ).normalized;
        Vector2 force = direction * (moveSpeed * 10) * Time.deltaTime;

        rigidbody2d.AddForce(force);

        float distance = Vector2.Distance(rigidbody2d.position, path.vectorPath[currentWayPoint]);

        if (distance <= nextWayPoinDistance)
            currentWayPoint++;
    }
}