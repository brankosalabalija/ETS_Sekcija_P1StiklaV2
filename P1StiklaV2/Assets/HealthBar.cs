using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    float scale;
    public int HP=50;
    public float MaxHP=50;
    void Update()
    {
        if(HP<=0)
        {
            this.transform.localScale=new Vector3(0,1f,1f);
            return;
        }
        this.transform.localScale=new Vector3((HP/MaxHP),1f,1f);
        
    }


    public void setLives(int l)
    {
        HP=l;
    }

    public void setTotalLives(int l)
    {
        MaxHP = l;
    }
}
