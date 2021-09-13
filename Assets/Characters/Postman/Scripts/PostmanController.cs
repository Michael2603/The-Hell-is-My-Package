using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PostmanController : MonoBehaviour
{
    public bool specialPostman = false; // Special postmans can shoot letters

    public Rigidbody2D rigidbody2d;
    Collider2D coll;
    Transform transform;
    Animator animator;
    public RuntimeAnimatorController[] animationControllers;

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

    int health = 3;

    [SerializeField] float shootTimer;
    float Timer;
    [SerializeField] Transform rotationTransform;
    [SerializeField] Transform letterEmitter;
    [SerializeField] GameObject letter;
    [SerializeField] float letterSpeed;
    bool canShoot = true;

    MapBrain mapBrain;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animationControllers[Random.Range(0, 4)];    
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        coll = GetComponent<Collider2D>();
        seeker = GetComponent<Seeker>();

        waitingPoints = GameObject.FindGameObjectsWithTag("WaitingPoint");
        deliveryPoints = GameObject.FindGameObjectsWithTag("DeliveryPoint");
        target = waitingPoints[Random.Range(0, waitingPoints.Length)].gameObject.GetComponent<Transform>();
        mapBrain = GameObject.Find("MapBrain").GetComponent<MapBrain>();

        InvokeRepeating("UpdatePath", 0, .5f);

        if (Random.Range(0,100) <= 15)
            specialPostman = true;
    }

    void FixedUpdate()
    {        
        switch (state)
        {
            case "TrackPackage":
                TrackPackage();
            break;
            case "DeliveryPackage":
                DeliveryPackage();
            break;
        }

        animator.SetFloat("MoveX", rigidbody2d.velocity.x);
        animator.SetFloat("MoveY", rigidbody2d.velocity.y);

        if (specialPostman)
            TrackPlayer();

        if (!canShoot) // This can limit the amount of bullets that are shooted
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                canShoot = true;
                Timer = shootTimer;
            }
        }

        if (this.health <= 0)
        {
            rigidbody2d.velocity = new Vector3(0,0,0);
            coll.enabled = false;
            specialPostman = false;
        }

        if ( rigidbody2d.velocity.x <= .01f )
            GetComponent<SpriteRenderer>().flipX = false;
        else if ( rigidbody2d.velocity.x >= -.01f )
            GetComponent<SpriteRenderer>().flipX = true;
    }

    void TrackPackage()
    {
        if (transform.childCount > 1)
        {
            state = "DeliveryPackage";
            target = waitingPoints[Random.Range(0, waitingPoints.Length)].gameObject.GetComponent<Transform>();
        }

        Collider2D checkZone = Physics2D.OverlapCircle(rigidbody2d.position, 10, 1 << LayerMask.NameToLayer("Box"));
        if (checkZone != null)
        {
            box = checkZone.gameObject.GetComponent<Transform>();
                    
            if ( box.GetComponent<BoxController>().Target(this.gameObject.GetInstanceID()) )
                target = box;
        }
        else
            box = null;

        GoAfterTarget();
    }

    void DeliveryPackage()
    {
        GoAfterTarget();

        if (reachedEndOfPath && transform.childCount == 0)
        {
            state = "TrackPackage";
            target = waitingPoints[Random.Range(0, waitingPoints.Length)].gameObject.GetComponent<Transform>();
        }
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
            
            state = "DeliveryPackage";
            target = deliveryPoints[Random.Range(0, deliveryPoints.Length)].GetComponent<Transform>();
        }

        if ( state == "DeliveryPackage" && (other.gameObject.tag == "DeliveryPoint") )
        {
            state = "TrackPackage";
        }
    }


    void TrackPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(rigidbody2d.position, 10, 1 << LayerMask.NameToLayer("Player"));
        if (player != null && canShoot)
        {
            Shoot( player.gameObject.GetComponent<Transform>() );
        }
    }

    void Shoot(Transform player)
    {
        // Returns player's angle relative to guard's position
        Vector3 relative = transform.InverseTransformPoint(player.position);
        float Angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        rotationTransform.rotation = Quaternion.Euler(Vector3.forward * Angle);

        GameObject tempLetter = Instantiate(this.letter, letterEmitter.position, rotationTransform.rotation);
        Transform tempLetterTransform = tempLetter.GetComponent<Transform>();
        tempLetter.GetComponent<Rigidbody2D>().AddForce(tempLetterTransform.right * letterSpeed, ForceMode2D.Impulse);

        animator.SetTrigger("Shoot");
        canShoot = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, 10);
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
        {
            currentWayPoint++;
            rigidbody2d.AddForce(-force);
        }
    }

    public void Hit()
    {
        this.health--;

        if (health <= 0)
            animator.SetTrigger("Dead");
        else
            animator.SetTrigger("Hit");

        mapBrain.SetInsanityLevel(.07f);

    }

    public void Dead()
    {
        Destroy(this.gameObject);
    }

    void HealthManager()
    {
        if (health <= 0)
        {
            animator.SetTrigger("Dead");
        }
    }
}