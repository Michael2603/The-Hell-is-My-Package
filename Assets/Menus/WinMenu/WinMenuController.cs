using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WinMenuController : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] VideoPlayer video;
    [SerializeField] GameObject winImage;

    public double time;
    public float currentTime = 3;

    void Start()
    {
        // time = video.GetComponent<VideoPlayer>().clip.length;
        winImage.SetActive(true);
        currentTime = 3;
    }
 
   
    void FixedUpdate() 
    {
        // currentTime = video.GetComponent<VideoPlayer>().time;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            UI.SetActive(true);
        }
    }
}
