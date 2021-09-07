using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSpawnControll : MonoBehaviour
{
    [SerializeField] GameObject P_guard;
    [SerializeField] GameObject S_guard;
    [SerializeField] GameObject R_guard;

    public void Spawn(string guardType)
    {
        if (guardType == "Pistol")
            Instantiate(P_guard, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
        else if (guardType == "Shotgun")
            Instantiate(S_guard, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
        if (guardType == "Rifle")
            Instantiate(R_guard, this.gameObject.GetComponent<Transform>().position, gameObject.transform.rotation);
    }
}
