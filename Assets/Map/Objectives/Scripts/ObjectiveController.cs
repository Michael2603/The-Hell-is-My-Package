using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ( other.gameObject.layer == LayerMask.NameToLayer("Box") )
        {
            other.GetComponent<Transform>().SetParent(this.GetComponent<Transform>());
            other.enabled = false;
        }
    }
}
