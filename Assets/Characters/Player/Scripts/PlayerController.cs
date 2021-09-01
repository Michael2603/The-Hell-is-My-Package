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

        animator.SetFloat("Move X", Input.GetAxis("Horizontal"));
        animator.SetFloat("Move Y", Input.GetAxis("Vertical"));

        MovementSystem();
        HealthManager();
        AudioManager();
    }

    void MovementSystem()
    {
        rigidbody2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);

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
            Vector3 dir = new Vector3(0,0,0);
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            if (moveX > 0)
                dir.x = .5f;
            else if (moveX < 0)
                dir.x = -.5f;
            
            if (moveY > 0)
                dir.y = .5f;
            else if (moveY < 0)
                dir.y = -.5f;

            RaycastHit2D ray = Physics2D.BoxCast(transform.position + dir, new Vector2(2,2), 0f, dir, .1f, 1 << LayerMask.NameToLayer("Box"));
            if ( ray )
            {
                boxCheckLocked = ray.transform.gameObject;
                if (!audio2.isPlaying)
                    audio2.Play();
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

    void AudioManager()
    {
        if (rigidbody2d.velocity.x != 0 || rigidbody2d.velocity.y != 0)
        {
            if (!audio.isPlaying)
                audio.Play();
        }
    }

    public void Hit(int amount)
    {
        this.health -= amount;
        animator.SetTrigger("Hit");

        if (this.health <= 2)
        {
            animator.SetTrigger("Dead");
            this.dead = true;
        }
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