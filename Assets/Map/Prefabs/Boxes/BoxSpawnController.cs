using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawnController : MonoBehaviour
{
    [SerializeField] GameObject miniPackage; //40%
    [SerializeField] GameObject smallPackage; //30%
    [SerializeField] GameObject mediumPackage; //20%
    [SerializeField] GameObject largePackage; //10%

    public void Spawn()
    {
        Instantiate(selectPackageType(Random.Range(0,10)), this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
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
