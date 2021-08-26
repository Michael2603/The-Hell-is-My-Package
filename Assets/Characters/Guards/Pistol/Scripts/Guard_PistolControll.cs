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

    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] Transform rotationTransform;
    [SerializeField] Transform bulletEmitter;
    [SerializeField] float shootTimer;
    float Timer;
    bool canShoot = true;

    public Transform target;
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
        Timer = shootTimer;
    
        coll = GetComponent<Collider2D>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0, .5f);
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

        if (!canShoot)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                Timer = shootTimer;
                canShoot = true;
            }
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
            Collider2D playerFound = Physics2D.OverlapCircle(transform.position, 10, 1 << LayerMask.NameToLayer("Player"));
            
            case "Focused":
                if (playerFound)
                {
                    if (canShoot)
                    {
                        Shoot(playerFound.GetComponent<Transform>());
                    }
                }
                else
                {
                    persuitType = "Searching";
                    target = playerFound;
                }

            break;
            case "Searching":
                Collider2D playerSearch = Physics2D.OverlapCircle(transform.position, 15, 1 << LayerMask.NameToLayer("Player"));

                //If the player is within an area, the guard starts to search for him and decreases the detectionMeter, else the guard stays still untill the detectionMeter is 0
                if (playerSearch)
                {
                    GoAfterTarget();
                    detectionRate -= Time.deltaTime * .4f;
                    Collider2D playerFound = Physics2D.OverlapCircle(transform.position, 10, 1 << LayerMask.NameToLayer("Player"));
                    if (playerFound)
                        persuitType = "Focused";
                }
                else
                {
                    detectionRate -= Time.deltaTime * .1f;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(GetComponent<Transform>().position, 10);
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
            currentWayPoint++;
    }
}
