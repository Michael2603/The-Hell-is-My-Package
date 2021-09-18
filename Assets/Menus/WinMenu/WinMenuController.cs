using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WinMenuController : MonoBehaviour
{
    [SerializeField] GameObject UI;
    [SerializeField] VideoPlayer video;

    public double time;
    public double currentTime;

    void Start()
    {
        time = video.GetComponent<VideoPlayer>().clip.length;
    }
 
   
    void Update() 
    {
        currentTime = video.GetComponent<VideoPlayer>().time;
        if (currentTime >= time) 
        {
            UI.SetActive(true);
        }
    }
}
