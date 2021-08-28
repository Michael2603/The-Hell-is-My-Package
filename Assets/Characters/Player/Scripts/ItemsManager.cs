using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public List<GameObject> itemsList = new List<GameObject>();

    public void PickPackage(GameObject item)
    {
        if (item.layer != 1 << LayerMask.NameToLayer("Box"))
        {
            itemsList.Add( new GameObject(item.name) );
            Destroy(item);
        }
    }
}
