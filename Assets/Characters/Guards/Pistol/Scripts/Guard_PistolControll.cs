using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Guard_PistolControll : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Collider2D collider2d;
    Transform transform;
    Animator animator;
    
    float detectionRate = 0;
    public string patrolType;
    string persuitType = "Focused";
    public float moveSpeed;
    bool lostContact = true;
    Transform playerTransform;
    int health = 3;

    GameObject patrolPost1;
    GameObject patrolPost2;
    GameObject[] Posts;

    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform rotationTransform;
    [SerializeField] Transform bulletEmitter;
    [SerializeField] float shootTimer;
    float Timer;
    bool canShoot = true;

    public Transform target;
    public Transform currentPost;
    public bool reachedPost;

    public float nextWayPoinDistance = 3f;
    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        Timer = shootTimer;

        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0, .5f);

        Posts = GameObject.FindGameObjectsWithTag("Post");
        
        patrolPost1 = Posts[Random.Range(0, Posts.Length)];
        patrolPost2 = Posts[Random.Range(0, Posts.Length)];

        currentPost = patrolPost1.GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        DetectPlayer();
        if (lostContact)
        {
            Patrol();
        }
        else
        {
            PlayerFocused();
        }

        if (!canShoot) // This can limit the amount of bullets that are shot
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = shootTimer;
                canShoot = true;
            }
        }

        animator.SetFloat("MoveX", rigidbody2d.velocity.x);
        animator.SetFloat("MoveY", rigidbody2d.velocity.y);
    }

    //Controls the detection mechanics
    void DetectPlayer()
    {
        slider.value = detectionRate;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        if (detectionRate > .6f)
            Debug.Log("I think I saw something strange...");

        // If finds the player, starts shooting him and call other guards close by
        if (detectionRate >= 1 && lostContact == true)
        {
            lostContact = false;

            Collider2D[] otherGuards = Physics2D.OverlapCircleAll(transform.position, 15, 1 << LayerMask.NameToLayer("Guard"));

            for (int i = 0; i < otherGuards.Length; i++)
            {
                string name = otherGuards[i].gameObject.name;
            
                if (name.Contains("Pistol"))
                    otherGuards[i].gameObject.GetComponent<Guard_PistolControll>().Called(playerTransform);
                else if (name.Contains("Rifle"))
                    otherGuards[i].gameObject.GetComponent<Guard_RifleControll>().Called(playerTransform);
                else if (name.Contains("Shotgun"))
                    otherGuards[i].gameObject.GetComponent<Guard_ShotgunControll>().Called(playerTransform);
            }

        }
        if (detectionRate <= 0)
        {
            lostContact = true;
            playerTransform = null;

            if (patrolType == "Route")
                target = currentPost;
        }
    }

    // Controls the 2 states of patrol (The first is the guard idle but with a high range track and the secound is the guard walking through a route but with a reduced sight)
    void Patrol()
    {
        switch(patrolType)
        {
            case "Idle":
                rigidbody2d.velocity = new Vector3 (0,0,0);

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

            // Walk through the map while check for the player;
            case "Route":
                Transform patrol1 = patrolPost1.GetComponent<Transform>();
                Transform patrol2 = patrolPost2.GetComponent<Transform>();

                target = currentPost;

                if(reachedPost)
                {
                    if (currentPost == patrol1)
                        currentPost = patrol2;
                    else if (currentPost == patrol2)
                        currentPost = patrol1;
                    
                    reachedPost = false;
                }
                GoAfterTarget();


                Collider2D checkZon = Physics2D.OverlapCircle(transform.position, 7, 1 << LayerMask.NameToLayer("Player"));
                if ( checkZon && detectionRate < 1)
                {
                    detectionRate += Time.deltaTime * .8f;
                }
                else if ( !checkZon && detectionRate > 0 )
                {
                    detectionRate -= Time.deltaTime * .1f;
                }
            break;
        }
    }

    // Controls the 2 states of persuit (when he focus and attack the player and when the player leaves his sight and he starts to search for the player)
    void PlayerFocused()
    {
        Collider2D playerFound = Physics2D.OverlapCircle(transform.position, 10, 1 << LayerMask.NameToLayer("Player"));
        
        if (playerFound != null)
            playerTransform = playerFound.gameObject.GetComponent<Transform>();

        switch (persuitType)
        {
            //Shoot the player i found
            case "Focused":

                if (playerFound)
                {
                    animator.SetBool("Shooting", true);
                    detectionRate = 1;
                    if (canShoot)
                        Shoot(playerFound.GetComponent<Transform>());
                }
                else
                {
                    persuitType = "Searching";
                    animator.SetBool("Shooting", false);
                    target = playerTransform;
                }
            break;
            // Starts to go after the player
            case "Searching":
                Collider2D playerSearch = Physics2D.OverlapCircle(transform.position, 15, 1 << LayerMask.NameToLayer("Player"));

                //If the player is within an area, the guard starts to search for him and decreases the detectionMeter, else the guard stays still untill the detectionMeter is 0
                if (playerSearch)
                {
                    GoAfterTarget();
                    detectionRate -= Time.deltaTime * .1f;
                    if (playerFound)
                        persuitType = "Focused";
                }
                else
                {
                    
                    detectionRate -= Time.deltaTime * .2f;
                    rigidbody2d.velocity = new Vector3(0,0,0);
                }
            break;
        }
    }

    void Shoot(Transform player)
    {
        // Returns player's angle relative to guard's position
        Vector3 relative = transform.InverseTransformPoint(player.position);
        float Angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
        rotationTransform.rotation = Quaternion.Euler(Vector3.forward * Angle);

        GameObject tempBullet = Instantiate(this.bullet, bulletEmitter.position, rotationTransform.rotation);
        Transform tempBulletTransform = tempBullet.GetComponent<Transform>();
        tempBullet.GetComponent<Rigidbody2D>().AddForce(tempBulletTransform.right * bulletSpeed, ForceMode2D.Impulse);

        canShoot = false;
    }

    public void ReachedPost()
    {
        reachedPost = true;
    }

    public void Called(Transform player)
    {
        this.detectionRate = 1;
        this.lostContact = false;
        this.target = player;
    }

    void HealthManager()
    {
        if (this.health <= 0)
        {
            animator.SetTrigger("Dead");
        }
    }

    public void Dead()
    {
        Destroy(this.gameObject);
    }

    public void Hit()
    {
        this.health -= 1;
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
        Vector2 force = direction * (moveSpeed) * Time.deltaTime;

        rigidbody2d.AddForce(force);

        float distance = Vector2.Distance(rigidbody2d.position, path.vectorPath[currentWayPoint]);

        if (distance <= nextWayPoinDistance)
        {
            currentWayPoint++;
            rigidbody2d.AddForce(-force);
        }
    }
}
