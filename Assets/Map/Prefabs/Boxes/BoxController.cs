using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    GameObject SD;
    int targetedID = 0;

    public bool playersBox = false;
    public ItemsManager items;

    ParticleSystem particles;

    void Start()
    {
        items = GameObject.Find("Player").GetComponent<ItemsManager>();
        particles = GetComponent<ParticleSystem>();
    }

    public void Openned()
    {
        if (playersBox)
            items.GotMyPackage();
            
        Destroy(this.gameObject);
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

    public void SelectedPackage()
    {
        this.playersBox = true;
    }

    public void Untarget(string name)
    {
        targetedID = 0;
    }

    public void StartParticle()
    {
        particles.Play();
    }
    public void StopParticle()
    {
        particles.Stop();
    }
}