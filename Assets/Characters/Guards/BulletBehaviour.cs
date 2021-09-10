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

        if ( GetComponent<Collider>().gameObject.layer == LayerMask.NameToLayer("Guard") )
        {
            if (GetComponent<Collider>().gameObject.tag == "Pistol")
                GetComponent<Collider>().gameObject.GetComponent<Guard_PistolControll>().Hit();
            else if (GetComponent<Collider>().gameObject.tag == "Rifle")
                GetComponent<Collider>().gameObject.GetComponent<Guard_RifleControll>().Hit();
            else if (GetComponent<Collider>().gameObject.tag == "Rifle")
                GetComponent<Collider>().gameObject.GetComponent<Guard_ShotgunControll>().Hit();
        }

        if ( GetComponent<Collider>().gameObject.layer == 1 << LayerMask.NameToLayer("Postman") )
            GetComponent<Collider>().gameObject.GetComponent<PostmanController>().Hit();
        
    }
}
