using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControll : MonoBehaviour
{
    [SerializeField] GameObject Postman;

    public float spawnTimer;

    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
            Spawn( Postman);
    }

    void Spawn(GameObject package)
    {
        Instantiate(Postman, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
        spawnTimer = 5;
    }
}
