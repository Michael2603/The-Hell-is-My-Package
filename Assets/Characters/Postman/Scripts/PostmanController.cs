using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostmanController : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rigidbody2d;
    Collider2D coll;
    Transform transform;
    Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] string state = "TrackPackage";
    Transform box;
    GameObject objective;
    
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rigidbody2d.velocity.x) > 0 || Mathf.Abs(rigidbody2d.velocity.y) > 0)
            animator.SetFloat("Velocity", 1);
        
        switch (state)
        {
            case "TrackPackage":
                TrackBox();
            break;
            case "PickBox":
                PickBox();
            break;
            case "DeliveryBox":
                DeliveryBox();
            break;
        }
    }

    void TrackBox()
    {
        Collider2D checkZone = Physics2D.OverlapCircle(rigidbody2d.position, 10, 1 << LayerMask.NameToLayer("Box"));
        if (checkZone != null)
        {
            state = "PickBox";
            box = checkZone.gameObject.GetComponent<Transform>();
        }else
        {
            box = null;
        }
    } 

    void PickBox()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), box.position, moveSpeed * Time.deltaTime);

        // Get the angle that the postman is pointing to by moving, using it, adds 2 check angles, one 45° upper and other 45° lower. These are used to check for walls and evade them
        Vector3 movement =  new Vector3(rigidbody2d.velocity.x, rigidbody2d.velocity.y, 0);
        float pointingAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

        Vector3 upperCast = Quaternion.AngleAxis(pointingAngle + 45 , Vector3.forward) * Vector3.right * 20;
        Vector3 lowerCast = Quaternion.AngleAxis(pointingAngle - 45, Vector3.forward) * Vector3.right * 20;

        // If a wall is detected, the postman will try to avoid it
        bool upperWall = Physics2D.Raycast( transform.position, upperCast, 10, 1 << LayerMask.NameToLayer("Wall") );
        if (upperWall == true){}
            // Debug.Log("Upper Wall");
        
        bool lowerWall = Physics2D.Raycast( transform.position, lowerCast, 10, 1 << LayerMask.NameToLayer("Wall") );
        if (lowerWall == true){}
            // Debug.Log("Lower Wall");

        if ( !upperWall && !lowerWall ){}
            // Debug.Log("No Wall");
    }

    void DeliveryBox()
    {
        Vector3 obectivePos = objective.GetComponent<Transform>().position;
        
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), obectivePos, moveSpeed * Time.deltaTime);
        if (transform.position == obectivePos)
        {
            state = "TrackPackage";
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if is looking for a box and if has collided to a box, if so, grab it
        if (state == "PickBox" && (other.gameObject.layer == LayerMask.NameToLayer("Box") ))
        {
            Transform boxTransform = box.gameObject.GetComponent<Transform>();
            Collider2D boxCollider = box.gameObject.GetComponent<Collider2D>();
            
            boxTransform.SetParent(this.transform);
            boxCollider.isTrigger = true;

            boxTransform.localPosition = new Vector3(.04f, 0, 0);
            objective = GameObject.Find("Objective");
            state = "DeliveryBox";
        }
    }

    // Pra determinar qual ângulo a personagem tá olhando enquanto anda eu posso pegar o Seno (ou cosseno n lembro) e fazer a soma deles cm os 45° de amplitude da checagemdas paredes

    // A checagem das paredes serão 2 raycast q ficam a 90° de diferença, sendo 45° acima do caminho sendo seguido e 45° abaixo. (Se um detectar uma parede, ele move um pouco mais pro outro lado)

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
}