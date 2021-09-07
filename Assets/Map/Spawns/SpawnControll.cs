using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControll : MonoBehaviour
{
    [SerializeField] GameObject Postman;

    public void Spawn()
    {
        Instantiate(Postman, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
    }
}