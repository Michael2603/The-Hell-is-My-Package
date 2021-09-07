using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    GameObject SD;
    int targetedID = 0;

    public GameObject Openned()
    {
        return SD;
    }

    // The first postman to target this box will lock it so no one else can target on it.
    public bool Target(int ID)
    {
        if (targetedID != 0)
        {
            if (targetedID != ID)
                return false;
            else
                return true;
        }
        else
            targetedID = ID;
            return true;
    }

    public void Untarget(string name)
    {
        targetedID = 0;
    }
}