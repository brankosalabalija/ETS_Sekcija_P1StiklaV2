using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownAnimation : MonoBehaviour
{

    Vector3 initPos;

    public float freq;
    public float abs;

    void Start()
    {
        initPos=transform.position;
    }


    void Update()
    {
        transform.position=new Vector3(initPos.x,initPos.y + (float)(Math.Sin(Time.time*freq)*abs),0f);
    }
}
