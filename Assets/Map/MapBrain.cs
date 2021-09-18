using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Universal;
using UnityEngine.SceneManagement;

public class MapBrain : MonoBehaviour
{
    [SerializeField] GameObject Lamps;
    public UnityEngine.Experimental.Rendering.Universal.Light2D globalLight;

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
    [SerializeField] List<BoxController> CandidateBoxes = new List<BoxController>();

    [SerializeField] GameObject PauseHud;
    [SerializeField] GameObject OptionsHud;
    [SerializeField] GameObject GameHud;
    [SerializeField] GameObject WinHud;

    [SerializeField] Slider soundBar;

    [SerializeField] GameObject LightingButton;
    [SerializeField] Sprite lightOn;
    [SerializeField] Sprite lightOff;
    public AudioSource music;

    [SerializeField] AudioSource selectedButton;

    void Start()
    {
        CandidateBoxes[Random.Range(0,CandidateBoxes.Count)].SelectedPackage();
        music.Play();
    }

    void Update()
    {
        // Adjust automatically the global sound volume and the lightin quality from options menu's configuratioins
        AudioListener.volume = UniversalScript.soundVolume;
        if (UniversalScript.betterLightin)
        {
            Lamps.SetActive(true);
            globalLight.intensity = .7f;
        }
        else
        {
            Lamps.SetActive(false);
            globalLight.intensity = 1.2f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ( PauseHud.activeSelf )
                this.BackToGame();
            else
                this.PauseMenu();
        }
    }

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
        else if (insanityLevel >= 3)
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

    public void OptionsMenu()
    {
        PauseHud.SetActive(false);
        OptionsHud.SetActive(true);
    }
    
    public void BackToGame()
    {
        PauseHud.SetActive(false);
        GameHud.SetActive(true);
    }

    public void PauseMenu()
    {
        GameHud.SetActive(false);
        PauseHud.SetActive(true);
        OptionsHud.SetActive(false);
    }

    public void SelectedSound()
    {
        selectedButton.Play();
    }

    public void ImprovedLighting()
    {
        UniversalScript.betterLightin = !UniversalScript.betterLightin;
        if ( UniversalScript.betterLightin )
            LightingButton.GetComponent<Image>().sprite = lightOn;
        else if ( !UniversalScript.betterLightin )
            LightingButton.GetComponent<Image>().sprite = lightOff;
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void MasterSound()
    {
        UniversalScript.soundVolume = soundBar.value;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Win()
    {
        foreach (AudioSource soundEmitter in GameObject.FindObjectsOfType<AudioSource>())
        {
            if (soundEmitter.gameObject.name != "Background")
                soundEmitter.enabled = false;
        }
        WinHud.SetActive(true);
    }
}
