using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    [SerializeField] GameObject MainMenuLayout;
    [SerializeField] GameObject OptionsLayout;
    [SerializeField] GameObject CreditsLayout;
    
    [SerializeField] GameObject LightingButton;
    public bool improvedLighting = true;
    public Sprite lightOn;
    public Sprite lightOff;

    [SerializeField] Slider soundBar;

    //Buttons
        public void LoadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OptionMenu()
        {
            MainMenuLayout.SetActive(false);
            OptionsLayout.SetActive(true);
        }
        
        public void CreditsMenu()
        {
            MainMenuLayout.SetActive(false);
            CreditsLayout.SetActive(true);
        }

        public void Options_Back()
        {
            OptionsLayout.SetActive(false);
            MainMenuLayout.SetActive(true);
        }
        
        public void Credits_Back()
        {
            CreditsLayout.SetActive(false);
            MainMenuLayout.SetActive(true);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void ImprovedLighting()
        {
            improvedLighting = !improvedLighting;
            if ( improvedLighting )
                LightingButton.GetComponent<Image>().sprite = lightOn;
            else if ( !improvedLighting )
                LightingButton.GetComponent<Image>().sprite = lightOff;
        }

    public void MasterSound()
    {
        AudioListener.volume = soundBar.value;
    }
}
