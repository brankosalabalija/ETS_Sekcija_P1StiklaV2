using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    float scale;
    int lives=200;
    void Update()
    {
        if(lives<=0)
        {
            this.transform.localScale=new Vector3(0,1f,1f);
            return;
        }
        this.transform.localScale=new Vector3((lives/200f),1f,1f);
        
    }


    public void setLives(int l)
    {
        lives=l;
    }
}
