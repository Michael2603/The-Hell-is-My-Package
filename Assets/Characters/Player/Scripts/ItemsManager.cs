using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public List<GameObject> itemsList = new List<GameObject>();

    public bool invoice = false;
    public bool myPackage = false;

    public void PickPackage(GameObject item)
    {
        if (item.layer != 1 << LayerMask.NameToLayer("Box"))
        {
            itemsList.Add( new GameObject(item.name) );
            Destroy(item);
        }
    }

    public void GotTheInvoice()
    {
        this.invoice = true;
    }

    public void GotMyPackage()
    {
        this.myPackage = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if ( myPackage == true && invoice == true )
        {
            if ( other.gameObject.layer == LayerMask.NameToLayer("Truck") && Input.GetMouseButton(1))
            {
                other.gameObject.GetComponent<TruckController>().Depart();
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<PlayerController>().enabled = false;
                GetComponent<PlayerController>().animator.SetTrigger("Idle");

            }

        }
    }
}
