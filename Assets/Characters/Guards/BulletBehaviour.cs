using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            other.gameObject.GetComponent<PlayerController>().Hit();

        if ( other.gameObject.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Guard") )
        {
            if (other.gameObject.GetComponent<Collider2D>().gameObject.tag == "Pistol")
                other.gameObject.GetComponent<Collider2D>().gameObject.GetComponent<Guard_PistolControll>().Hit();
            if (other.gameObject.GetComponent<Collider2D>().gameObject.tag == "Rifle")
                other.gameObject.GetComponent<Collider2D>().gameObject.GetComponent<Guard_RifleControll>().Hit();
            if (other.gameObject.GetComponent<Collider2D>().gameObject.tag == "Shotgun")
                other.gameObject.GetComponent<Collider2D>().gameObject.GetComponent<Guard_ShotgunControll>().Hit();
        }

        if ( GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Postman") )
            GetComponent<Collider2D>().gameObject.GetComponent<PostmanController>().Hit();

        Destroy(this.gameObject);
    }
}
