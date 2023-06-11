using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScavBulletScript : MonoBehaviour
{

    public Vector2 MovingDirection;
    public Vector2 thisPos;
    
    private Stopwatch _timer;
    private bool flipyBullet;
 

    // Start is called before the first frame update
    void Start()
    {
        _timer = new Stopwatch();
        _timer.Start();
        flipyBullet = GetComponent<SpriteRenderer>().flipY;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = (MovingDirection - thisPos).normalized * 8f;

        if (_timer.ElapsedMilliseconds > 10000)
        {
            _timer.Stop();
            Destroy(this.gameObject);
        }

        if ((_timer.ElapsedMilliseconds % 50) == 0)
        {
            if (flipyBullet)
            {
                GetComponent<SpriteRenderer>().flipY = false;
                flipyBullet = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipY = true;
                flipyBullet = true;
            }
        }
    }

    public void SetDir(Vector2 dir)
    {
        MovingDirection = dir;
        thisPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("Scav Bullet trigger detection with: " + other.ToString());
        if ((other.gameObject.layer != 7) && (other.gameObject.layer != 8))
            Destroy(this.gameObject);
    }
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    UnityEngine.Debug.Log("Scav Bullet collision detection with: " + other.ToString());
    //    if ((other.gameObject.layer != 7) && (other.gameObject.layer != 8))
    //        Destroy(this.gameObject);
    //}
}
