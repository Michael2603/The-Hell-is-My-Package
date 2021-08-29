using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnController : MonoBehaviour
{
    [SerializeField] GameObject miniPackage; //40%
    [SerializeField] GameObject smallPackage; //30%
    [SerializeField] GameObject mediumPackage; //20%
    [SerializeField] GameObject largePackage; //10%

    public float spawnTimer;

    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
            Spawn( selectPackageType(Random.Range(1, 11)) );
    }

    void Spawn(GameObject package)
    {
        Instantiate(package, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
        spawnTimer = 5;
    }

    GameObject selectPackageType(int num)
    {
        if ( 0 < num && num < 4 )
            return miniPackage;
        else if ( 4 < num && num < 7 )
            return smallPackage;
        else if ( 7 < num && num < 9 )
            return mediumPackage;
        else 
            return largePackage;
    }
}
