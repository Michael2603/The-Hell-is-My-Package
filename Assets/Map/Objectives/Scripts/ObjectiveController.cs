using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if ( otherCollider.gameObject.layer == LayerMask.NameToLayer("Box") )
        {
            Transform boxPos = otherCollider.GetComponent<Transform>();

            boxPos.SetParent(this.GetComponent<Transform>());
            otherCollider.enabled = false;


            float objectiveSizeX = this.GetComponent<Transform>().localScale.x;
            float objectiveSizeY = this.GetComponent<Transform>().localScale.y;
            Vector3 randomPosition = new Vector3(Random.Range( -.8f, .8f / 2), Random.Range(- .8f, .8f / 2), 0 );
            
            boxPos.localPosition = randomPosition;
            As caixas estão indo longe



        }
    }
}
