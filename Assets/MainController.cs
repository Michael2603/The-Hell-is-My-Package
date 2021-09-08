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

    public void MasterSound()
    {
        AudioListener.volume = soundBar.value;
    }
}
