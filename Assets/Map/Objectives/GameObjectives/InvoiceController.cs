using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvoiceController : MonoBehaviour
{
    public ItemsManager itens;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            itens.GotTheInvoice();
            Destroy(this.gameObject);
        }
    }
}
