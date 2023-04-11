using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class ShowHidePlatforms : MonoBehaviour
{
    public int Cycle = 0;
    public int Secondss;

    private Stopwatch timer = new Stopwatch();
    private BoxCollider2D coll;
    private SpriteRenderer rend;
    
    
    void Start()
    {
        coll = this.GetComponent<BoxCollider2D>();
        rend = this.GetComponent<SpriteRenderer>();
        if (Cycle == 1)
            coll.enabled = true;
        else
            coll.enabled = false;
        timer.Start();
    }

    
    void Update()
    {
       
        if(timer.Elapsed.Seconds>=Secondss)
        {
            coll.enabled = !coll.enabled;
            
            timer.Restart();
        }
        if(!coll.enabled)
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b,0.50f);
        }
        else
        {
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1f);

        }
    }
}
