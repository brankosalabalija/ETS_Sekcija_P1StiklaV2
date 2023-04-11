using System;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScore : MonoBehaviour
{
    public Text Score;
    public Text Keys;
    public Text Time;
    private int playerScore;
    private int keyNum;
    private Stopwatch timeStart;
    void Start()
    {
        playerScore = 0;
        keyNum = 0;
        Score.text="Score: "+ playerScore;
        timeStart=new Stopwatch();
        timeStart.Start();
        
    }

    private void Update() {
        Time.text=String.Format("Time:{0:00}:{1:00}:{2:00}",timeStart.Elapsed.Minutes,timeStart.Elapsed.Seconds,timeStart.Elapsed.Milliseconds);
        Score.text="Score: "+playerScore;
        Keys.text="Keys: "+keyNum;
        if(keyNum==0)
        {
            Keys.enabled=false;
        }
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
            Keys.enabled=true;
            keyNum++;
            Keys.text="Keys: "+keyNum;
            Destroy(other.gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name=="KeyDoor")
        {
            if(keyNum>0)
            {
                keyNum--;
                Destroy(other.gameObject);
            }
        }
    }
}
