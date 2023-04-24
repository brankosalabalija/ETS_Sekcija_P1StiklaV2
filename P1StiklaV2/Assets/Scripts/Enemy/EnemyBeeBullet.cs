using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeeBullet : MonoBehaviour
{
    public Vector2 MovingDirection;
    public Vector2 thisPos;
    void Start()
    {
        
    }

    
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity=(MovingDirection-thisPos).normalized*8f;
    }

    public void SetDir(Vector2 dir)
    {
        MovingDirection=dir;
        thisPos=transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.layer!=7)
            Destroy(this.gameObject);
    }
}
