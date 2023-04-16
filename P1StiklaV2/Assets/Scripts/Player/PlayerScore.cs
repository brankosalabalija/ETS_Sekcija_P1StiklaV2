using System;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScore : MonoBehaviour
{
    public GameObject _HUD;
    public HUDScript _HUDScript;
    private int playerScore;
    private int keyNum;
    private Stopwatch timeStart;
    void Start()
    {
        playerScore = 0;
        keyNum = 0;
        timeStart=new Stopwatch();
        timeStart.Start();
        _HUDScript=_HUD.GetComponent<HUDScript>();
    }

    private void Update() {
        _HUDScript.UISetScore(playerScore);
        _HUDScript.UISetTime(timeStart.Elapsed.Minutes,timeStart.Elapsed.Seconds,timeStart.Elapsed.Milliseconds);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        string triggerName = other.name.ToUpper();
        if (triggerName == "COIN")
        {
            playerScore++;
            Destroy(other.gameObject);
        }
        if(triggerName=="BIGCOIN")
        {
            playerScore+=5;
            Destroy(other.gameObject);
        }
        if(triggerName=="KEY")
        {
            _HUDScript.UIGetKey();
            keyNum++;
            Destroy(other.gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name=="KeyDoor")
        {
            if(keyNum>0)
            {
                _HUDScript.UILoseKey();
                keyNum--;
                Destroy(other.gameObject);
            }
        }
    }
}
