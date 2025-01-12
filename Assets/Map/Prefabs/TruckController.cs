using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Transform transform;
    public MapBrain mapBrain;

    public AudioClip departSound;
    bool departing = false;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Update() {
        if (departing)
        {
            Depart();
            if ( transform.position.x <= -100 || transform.position.x >= 115)
            {
                mapBrain.Win();
            }
        }
    }

    public void Depart()
    {
        departing = true;
        
        rigidbody2d.constraints = RigidbodyConstraints2D.None;
        rigidbody2d.AddForce(transform.up * -3);

        AudioSource audioS = GetComponent<AudioSource>();

        audioS.clip = departSound;
        if (!audioS.isPlaying)
            audioS.Play();
    }
}