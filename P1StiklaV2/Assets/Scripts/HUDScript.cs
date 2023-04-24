using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public GameObject Panel;
    public Text ScoreUI;
    int ScoreNum;

    public Text TimeUI;
    string TimeFormated;

    public Image Key;
    bool KeyShown;
    public Image Hearth1;
    public Image Hearth2;
    public Image Hearth3;

    private Image[] Hearths;
    private int lives=3;

    bool isPaused;

    void Start()
    {
        ScoreNum = 0;
        TimeFormated = "Time: 00:00:00";
        Hearths = new Image[3];
        Hearths[2] = Hearth1;
        Hearths[1] = Hearth2;
        Hearths[0] = Hearth3;
        KeyShown=false;
        isPaused=false;
        Time.timeScale=1;
    }

    void Update()
    {
        TimeUI.text=TimeFormated;
        ScoreUI.text="Score: "+ScoreNum;
        if(KeyShown)
            Key.enabled=true;
        else
            Key.enabled=false;
    }

    public void Pause() {
        if(!isPaused)
            PauseGame();
        else
            UnPauseGame();
        }
    

    public void PauseGame()
    {
        isPaused=true;
        Panel.SetActive(true);
        Time.timeScale=0;
    }

    public void UnPauseGame()
    {
        isPaused=false;
        Time.timeScale=1;
        Panel.SetActive(false);
    }

    public void FadeOUT()
    {
        GetComponentInChildren<Animator>().SetTrigger("FadeOut");
    }

    public void UIReset()
    {
        Hearths[2].enabled=true;
        Hearths[1].enabled=true;
        Hearths[0].enabled=true;
        lives=3;
    }



    public void QuitGame()
    {
        Application.Quit();
    }
    public void UIGetKey()
    {
        KeyShown=true;
    }

    public void UILoseKey()
    {
        KeyShown=false;
    }

    public void UILoseLife()
    {
        lives--;
        Hearths[lives].enabled=false;
    }

    public void UISetScore(int score)
    {
        ScoreNum=score;
    }

    public void UISetTime(int minutes,int seconds,int miliseconds)
    {
        TimeFormated=String.Format("Time: {0:00}:{1:00}:{2:00}",minutes,seconds,miliseconds);
    }

    

}
