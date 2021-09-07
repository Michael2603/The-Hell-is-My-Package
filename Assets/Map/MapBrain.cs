using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBrain : MonoBehaviour
{
    [SerializeField] float insanityLevel = 0;
    public void SetInsanityLevel(float amount)
    {
        this.insanityLevel += amount;
    }
    
    float guardTimer = 0;
    float postmanTimer = 0;
    float boxTimer = 0;

    [SerializeField] List<SpawnControll> postmanSpawnsControll = new List<SpawnControll>();
    [SerializeField] GuardSpawnControll guardsSpawnControll;
    [SerializeField] List<BoxSpawnController> boxSpawnControllers = new List<BoxSpawnController>();

    void FixedUpdate()
    {
        insanityLevelBrain();

        if (insanityLevel >= 1)
        {
            guardTimer -= Time.deltaTime;
            postmanTimer -= Time.deltaTime;
            boxTimer -= Time.deltaTime;
        }
    }

    void insanityLevelBrain()
    {
        if (insanityLevel >= 1 && insanityLevel < 2)
        {
            if (guardTimer <= 0) // Spanw a guard
            {
                guardsSpawnControll.Spawn("Pistol");
                guardTimer = 20;
            }
            if (postmanTimer <= 0) // Spanw a postman
            {
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanTimer = 15;
            }
            if (boxTimer <= 0) // Spanw two boxes
            {
                boxSpawnControllers[Random.Range(0, boxSpawnControllers.Count)].Spawn();
                boxSpawnControllers[Random.Range(0, boxSpawnControllers.Count)].Spawn();
                boxTimer = 20;
            }
        }
        else if (insanityLevel >= 2 && insanityLevel < 3)
        {
            if (guardTimer <= 0) // Spanw a guard
            {
                guardsSpawnControll.Spawn("Pistol");
                guardTimer = 15;
            }
            if (postmanTimer <= 0) // Spanw three postman
            {
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanTimer = 20;
            }
            if (boxTimer <= 0) // Spanw four boxes
            {
                boxSpawnControllers[0].Spawn();
                boxSpawnControllers[1].Spawn();
                boxSpawnControllers[2].Spawn();
                boxSpawnControllers[3].Spawn();
                boxTimer = 20;
            }
        }
        else if (insanityLevel >= 3 && insanityLevel < 4)
        {
            if (guardTimer <= 0) // Spanw a guard
            {
                guardsSpawnControll.Spawn("Pistol");
                guardTimer = 12;
            }
            if (postmanTimer <= 0) // Spanw three postman
            {
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanSpawnsControll[Random.Range(0, postmanSpawnsControll.Count)].Spawn();
                postmanTimer = 20;
            }
            if (boxTimer <= 0) // Spanw two boxes
            {
                boxSpawnControllers[0].Spawn();
                boxSpawnControllers[1].Spawn();
                boxSpawnControllers[2].Spawn();
                boxSpawnControllers[3].Spawn();
                boxTimer = 15;
            }
        }
    }
}
