using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawnControll : MonoBehaviour
{
    [SerializeField] GameObject P_guard;
    [SerializeField] GameObject S_guard;
    [SerializeField] GameObject R_guard;

    public void Spawn(GameObject guardType)
    {
        Instantiate(guardType, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
    }
}
