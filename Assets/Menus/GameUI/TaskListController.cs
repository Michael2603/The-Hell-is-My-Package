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

    public GameObject extensionNote;
    public GameObject packageImage;
    public GameObject camera2;

    bool down = true;

    void Update()
    {
        if ( itemsMg.invoice )
        {
            check1.SetActive(true);
            extensionNote.SetActive(true);
        }
        
        if ( itemsMg.myPackage )
        {
            check2.SetActive(true);
            extensionNote.SetActive(false);
        }

        if ( itemsMg.invoice && itemsMg.myPackage )
            checkBox3.SetActive(true);

        if ( itemsMg.sent )
            check3.SetActive(true);

        Rigidbody2D rigidbody2d= GetComponent<Rigidbody2D>();
        if ( !down )
        {
            if ( GetComponent<RectTransform>().localPosition.y <= 690 )
            {
                rigidbody2d.velocity = new Vector3(rigidbody2d.velocity.x, 609, 0);
            }
            else
            {
                rigidbody2d.velocity = new Vector3(rigidbody2d.velocity.x, 0, 0);
            }
        }
        else
        {
            if ( GetComponent<RectTransform>().localPosition.y >= 334 )
            {
                rigidbody2d.velocity = new Vector3(rigidbody2d.velocity.x, -709, 0);
            }
            else
            {
                rigidbody2d.velocity = new Vector3(rigidbody2d.velocity.x, 0, 0);
            }
        }
    }

    public void Drop()
    {
        down = true;
    }
    
    public void Hide()
    {
        down = false;
    }
}
