using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if ( collider.gameObject.layer == LayerMask.NameToLayer("Bullet") )
        {
            Transform bltTransform = collider.gameObject.GetComponent<Transform>();
            Rigidbody2D bltRigidbody2d = collider.gameObject.GetComponent<Rigidbody2D>();

            
            // This means that the bullet is going mainly horizontal
            if (Mathf.Abs(bltRigidbody2d.velocity.x) > Mathf.Abs(bltRigidbody2d.velocity.y))
            {
                bltRigidbody2d.velocity = new Vector3(- bltRigidbody2d.velocity.x, bltRigidbody2d.velocity.y, 0);
                bltTransform.localScale = new Vector3(- bltTransform.localScale.x, bltTransform.localScale.y, 0);
            }
            // This means that the bullet is going mainly vertical
            if (Mathf.Abs(bltRigidbody2d.velocity.y) > Mathf.Abs(bltRigidbody2d.velocity.x))
            {
                bltRigidbody2d.velocity = new Vector3(bltRigidbody2d.velocity.x, - bltRigidbody2d.velocity.y, 0);
                bltTransform.localScale = new Vector3(bltTransform.localScale.x, -bltTransform.localScale.y, 0);
            }

        }

        if ( collider.gameObject.layer == LayerMask.NameToLayer("Guard") )
        {
            if (collider.gameObject.tag == "Pistol")
                collider.gameObject.GetComponent<Guard_PistolControll>().Hit();
            else if (collider.gameObject.tag == "Rifle")
                collider.gameObject.GetComponent<Guard_RifleControll>().Hit();
            else if (collider.gameObject.tag == "Shotgun")
                collider.gameObject.GetComponent<Guard_ShotgunControll>().Hit();
        }

        if ( collider.gameObject.layer == LayerMask.NameToLayer("Postman") )
            collider.gameObject.GetComponent<PostmanController>().Hit();
    }
}
