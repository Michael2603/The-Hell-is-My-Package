using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Contains("Bullet") )
        {
            Transform bltTransform = collider.gameObject.GetComponent<Transform>();
            Rigidbody2D bltRigidbody2d = collider.gameObject.GetComponent<Rigidbody2D>();

            if (Mathf.Abs(bltRigidbody2d.velocity.x) > Mathf.Abs(bltRigidbody2d.velocity.y))
                bltRigidbody2d.velocity = new Vector3(- bltRigidbody2d.velocity.x,bltRigidbody2d.velocity.y, 0);
            if (Mathf.Abs(bltRigidbody2d.velocity.y) > Mathf.Abs(bltRigidbody2d.velocity.x))
                bltRigidbody2d.velocity = new Vector3(bltRigidbody2d.velocity.x, - bltRigidbody2d.velocity.y, 0);
        }

        if ( collider.gameObject.layer == 1 << LayerMask.NameToLayer("Guard") )
        {
            if (name.Contains("Pistol"))
                collider.gameObject.GetComponent<Guard_PistolControll>().Hit();
            else if (name.Contains("Rifle"))
                collider.gameObject.GetComponent<Guard_RifleControll>().Hit();
            else if (name.Contains("Shotgun"))
                collider.gameObject.GetComponent<Guard_ShotgunControll>().Hit();
        }

        if ( collider.gameObject.layer == 1 << LayerMask.NameToLayer("Postman") )
        {
            collider.gameObject.GetComponent<PostmanController>().Hit();
        }
    }
}
