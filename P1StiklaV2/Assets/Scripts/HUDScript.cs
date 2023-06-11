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

    public GameObject key;
    //bool KeyShown;
    public GameObject heart;
    /*public Image Hearth1;
    public Image Hearth2;
    public Image Hearth3;*/

    public GameObject[] Hearts;
    public GameObject[] Keys;
    private int lives = 3;
    public int collectedKey = 0;

    bool isPaused;

    void Start()
    {
        ScoreNum = 0;
        TimeFormated = "Time: 00:00:00";
        Hearts = new GameObject[lives];
        Keys = new GameObject[5];
        InstantiateHeartsUI(lives);
        InstantiateKeysUI();
        /*Hearths[2] = Hearth1;
        Hearths[1] = Hearth2;
        Hearths[0] = Hearth3;*/
        //KeyShown=false;
        isPaused=false;
        Time.timeScale=1;
    }


    void Update()
    {
        TimeUI.text=TimeFormated;
        ScoreUI.text="Score: "+ScoreNum;
        //if(KeyShown)
        //    key.SetActive(true);
        //else
        //    key.SetActive(false);
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
        InstantiateHeartsUI(lives);
        collectedKey = 0;
        InstantiateKeysUI();
        /*Hearths[2].enabled=true;
        Hearths[1].enabled=true;
        Hearths[0].enabled=true;
        lives=3;*/
    }



    public void QuitGame()
    {
        Application.Quit();
    }
    public void UIGetKey()
    {
        collectedKey++;
        Keys[collectedKey].SetActive(true);
    }

    public void UILoseKey()
    {
        collectedKey--;
        Keys[collectedKey].SetActive(false);

    }

    public void UILoseLife()
    {
        lives--;
        Hearts[lives].SetActive(false);
    }

    public void UISetScore(int score)
    {
        ScoreNum=score;
    }

    public void UISetTime(int minutes,int seconds,int miliseconds)
    {
        TimeFormated=String.Format("Time: {0:00}:{1:00}:{2:00}",minutes,seconds,miliseconds);
    }

    public void InstantiateHeartsUI (int lives)
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            Hearts[i] = Instantiate(heart, heart.GetComponentInParent<Canvas>().transform);
            Hearts[i].SetActive(true);

            Vector3 offset = new Vector3(heart.transform.position.x + (i * 50f), heart.transform.position.y, 0);
            Hearts[i].transform.position = offset;

        }
    }

    private void InstantiateKeysUI()
    {
        int keysLength = 4;
        for (int i = 0; i < keysLength; i++)
        {
            Keys[i] = Instantiate(key, key.GetComponentInParent<Canvas>().transform);
            Keys[i].SetActive(false);

            Vector3 offset = new Vector3(key.transform.position.x + (i * 35f), key.transform.position.y, 0);
            Keys[i].transform.position = offset;
        }
    }
}
