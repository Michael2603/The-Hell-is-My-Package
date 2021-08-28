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


            Vector2 objectiveSize = this.GetComponent<BoxCollider2D>().size;
            Vector3 randomPosition = new Vector3(Random.Range( -objectiveSize.x, objectiveSize.x / 2), Random.Range(- objectiveSize.y, objectiveSize.y / 2), 0 );
            
            boxPos.localPosition = randomPosition;
        }

        if (otherCollider.gameObject.layer == LayerMask.NameToLayer("Guard"))
        {
            Transform guardTransform = otherCollider.GetComponent<Transform>();
            string name = otherCollider.gameObject.name;
            
            if (name.Contains("Pistol"))
                otherCollider.gameObject.GetComponent<Guard_PistolControll>().ReachedPost();
            else if (name.Contains("Rifle"))
                otherCollider.gameObject.GetComponent<Guard_RifleControll>().ReachedPost();
            else if (name.Contains("Shotgun"))
                otherCollider.gameObject.GetComponent<Guard_ShotgunControll>().ReachedPost();
        }
    }
}
