using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListController : MonoBehaviour
{
    public ItemsManager itemsMg;

    public GameObject check1;
    public GameObject check2;

    public GameObject checkBox3;
    public GameObject check3;

    void Update()
    {
        if ( itemsMg.invoice )
            check1.SetActive(true);
        
        if ( itemsMg.myPackage )
            check2.SetActive(true);

        if ( itemsMg.invoice && itemsMg.myPackage )
            checkBox3.SetActive(true);

        if ( itemsMg.sent )
            check3.SetActive(true);
    }
}
