using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().Hit();
        }

        if ( GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Guard") )
        {
            if (GetComponent<Collider2D>().gameObject.tag == "Pistol")
                GetComponent<Collider2D>().gameObject.GetComponent<Guard_PistolControll>().Hit();
            else if (GetComponent<Collider2D>().gameObject.tag == "Rifle")
                GetComponent<Collider2D>().gameObject.GetComponent<Guard_RifleControll>().Hit();
            else if (GetComponent<Collider2D>().gameObject.tag == "Rifle")
                GetComponent<Collider2D>().gameObject.GetComponent<Guard_ShotgunControll>().Hit();
        }

        if ( GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("Postman") )
            GetComponent<Collider2D>().gameObject.GetComponent<PostmanController>().Hit();
        
    }
}
