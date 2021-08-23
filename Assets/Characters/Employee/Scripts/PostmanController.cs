using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostmanController : MonoBehaviour
{
    public Rigidbody2D rigidbody2d;
    Transform transform;
    Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] string state = "TrackingPackage";
    Transform boxPos;
    
    void Awake()
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
        }else
        {
            boxPos = null;
        }
    } 

    void PackingBox()
    {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), boxPos.position, moveSpeed * Time.deltaTime);

        // Collider2D UpperRaycast = Physics2D.Raycast(transform.position, )
    }

    // Pra determinar qual ângulo a personagem tá olhando enquanto anda eu posso pegar o Seno (ou cosseno n lembro) e fazer a soma deles cm os 45° de amplitude da checagemdas paredes

    // A checagem das paredes serão 2 raycast q ficam a 90° de diferença, sendo 45° acima do caminho sendo seguido e 45° abaixo. (Se um detectar uma parede, ele move um pouco mais pro outro lado)

    void OnDrawGizmosSelected()
    {
        Rigidbody2D rrr = GetComponent<Rigidbody2D>();

        Debug.Log(rrr.velocity);

        Gizmos.DrawWireSphere(GetComponent<Transform>().position, 10);

        float movX = rigidbody2d.velocity.x;
        
        float movY = rigidbody2d.velocity.y;
        

        Vector3 movement =  new Vector3(movX, movY, 0);
        float newAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;


        Vector3 upperCast = Quaternion.AngleAxis(newAngle , Vector3.forward) * Vector3.right * 20;
        Vector3 lowerCast = Quaternion.AngleAxis(-45, Vector3.forward) * Vector3.right * 20; 


        Debug.Log( "Movement = " + movement );
        Debug.Log("newAngle = " + newAngle);


        Gizmos.DrawRay(this.transform.position, upperCast);
        Gizmos.DrawRay(this.transform.position, lowerCast);
        

    }       
}
