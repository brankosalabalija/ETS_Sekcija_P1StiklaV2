using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private int Lives;
    void Start()
    {
        Lives=3;
    }

    void Update()
    {
        if(Lives<=0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="PlayerProjectile")
        {
            Lives--;
        }
    }


}
