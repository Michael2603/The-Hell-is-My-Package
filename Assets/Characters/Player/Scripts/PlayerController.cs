using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    ItemsManager itemsMg;

    Rigidbody2D rigidbody2d;
    Transform transform;
    [SerializeField] float moveSpeed;
    Animator animator;

    [HideInInspector] public bool canReceiveInput;
    [HideInInspector] public bool inputReceived;

    public AudioSource audio;
    public AudioSource audio2;

    public AudioClip atk1;
    public AudioClip atk2;
    public AudioClip checkBox;
    public AudioClip takeDamage1;
    public AudioClip takeDamage2;
    public AudioClip takeDamage3;

    public float checkBoxTimer;
    GameObject boxCheckLocked;

    [Header ("Attack Colliders")]
    [SerializeField] Collider2D SideAtk1;
    [SerializeField] Collider2D SideAtk2;
    [SerializeField] Collider2D SideAtk3;
    [SerializeField] Collider2D FrontAtk1;
    [SerializeField] Collider2D FrontAtk2;
    [SerializeField] Collider2D FrontAtk3;
    [SerializeField] Collider2D BackAtk;


    int health = 5;
    bool dead = false;
    [SerializeField] Slider slider;
    
    public Vector3 direction; // Used to keep the direction the player is looking at
    float moveX;
    float moveY;
    float idleTimer;


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        itemsMg = GetComponent<ItemsManager>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (dead)
        {
            rigidbody2d.velocity = new Vector3(0,0,0);
            GetComponent<Collider2D>().enabled = false;
            return;
        }
 
        if (rigidbody2d.velocity.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }
        else if (rigidbody2d.velocity.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 0);
        }

        // Steps sound
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!audio.isPlaying)
                audio.Play();
        }

        
        MovementSystem();
        HealthManager();
    }

    void MovementSystem()
    {
        rigidbody2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        VerifyDirection();

        //Attacks
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
        // Check the package's content
        if (Input.GetMouseButton(1))
        {
            // Creates a boxCast to check if there's a package to check in the direction that the player is looking
            RaycastHit2D ray = Physics2D.BoxCast(transform.position + direction, new Vector2(2,2), 0f, direction, .1f, 1 << LayerMask.NameToLayer("Box"));
            if ( ray )
            {
                boxCheckLocked = ray.transform.gameObject;
                if (!audio2.isPlaying)
                {
                    PlaySound("CheckBox");
                }
            }

            if (boxCheckLocked != null)
            {
                CheckPackage(boxCheckLocked);
                rigidbody2d.velocity = new Vector3(0,0,0);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            checkBoxTimer = 5;
            boxCheckLocked = null;
            if (audio2.isPlaying)
                audio2.Stop();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 direction = new Vector3(0,0,0);
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            if (moveX > 0)
                direction.x = .5f;
            else if (moveX < 0)
                direction.x = -.5f;
            
            if (moveY > 0)
                direction.y = .5f;
            else if (moveY < 0)
                direction.y = -.5f;

            RaycastHit2D package = Physics2D.BoxCast(transform.position + direction, new Vector2(2,2), 0f, direction, .1f, 1 << LayerMask.NameToLayer("Box"));
            if ( package )
                itemsMg.PickPackage(package.transform.gameObject);
        }
    }

    void HealthManager()
    {
        slider.value = health;
    }

    void CheckPackage(GameObject box)
    {
        if (box.name.Contains("Big"))
            checkBoxTimer -= Time.deltaTime * .4f;
        if (box.name.Contains("Medium"))
            checkBoxTimer -= Time.deltaTime * .7f;
        if (box.name.Contains("Small"))
            checkBoxTimer -= Time.deltaTime;

        if (checkBoxTimer <= 0)
        {
            box.GetComponent<BoxController>().Openned();
        }
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            
            case "CheckBox":
                audio2.clip = checkBox;
            break;
            case "Damaged":
                int dmgSound = Random.Range(0,3);
                if (dmgSound == 0)
                    audio2.clip = takeDamage1;
                else if (dmgSound == 1)
                    audio2.clip = takeDamage2;
                else
                    audio2.clip = takeDamage3;
            break;
            case "Attack 1/3":
                audio2.clip = atk1;
            break;
            case "Attack 2":
                audio2.clip = atk2;
            break;

        }
        audio2.Play();
    }

    public void Hit()
    {
        this.health--;
        animator.SetTrigger("Hit");
        PlaySound("Damaged");

        if (this.health <= 2)
        {
            animator.SetTrigger("Dead");
            this.dead = true;
        }
    }

    // Check which direction is the player focused on, and if is not moving, plays the Idle animation (but keeps the focused direction)
    void VerifyDirection()
    {
        if ( (Input.GetAxis("Horizontal") > Input.GetAxis("Vertical")) && (Input.GetAxis("Horizontal") > 0) )
        {
            direction.x = .5f;
            direction.y = 0;
        }
        if ( (Input.GetAxis("Vertical") > Input.GetAxis("Horizontal")) && (Input.GetAxis("Vertical") > 0))
        {
            direction.y = .5f;
            direction.x = 0;
        }

        if ( (Input.GetAxis("Horizontal") < Input.GetAxis("Vertical")) && (Input.GetAxis("Horizontal") < 0) )
        {
            direction.x = -.5f;
            direction.y = 0;
        }
        if ( (Input.GetAxis("Vertical") < Input.GetAxis("Horizontal")) && (Input.GetAxis("Vertical") < 0))
        {
            direction.y = -.5f;
            direction.x = 0;
        }

        animator.SetFloat("Move X", direction.x);
        animator.SetFloat("Move Y", direction.y);

        if ( (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) )
            animator.SetBool("Idle", true);
        else
            animator.SetBool("Idle", false);
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