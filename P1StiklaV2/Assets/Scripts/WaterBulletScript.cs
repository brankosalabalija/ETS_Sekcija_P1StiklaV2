using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WaterBulletScript : MonoBehaviour
{
    public Vector2 MovingDirection;
    public Vector2 thisPos;

    private Stopwatch _timer;


    // Start is called before the first frame update
    void Start()
    {
        _timer = new Stopwatch();
        _timer.Start();
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
    }

    public void SetDir(Vector2 dir)
    {
        MovingDirection = dir;
        thisPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        UnityEngine.Debug.Log("Water Bullet collision detection with: " + other.gameObject.name);
        if ((other.gameObject.layer != 7) && (other.gameObject.layer != 8))
            Destroy(this.gameObject);
    }
}
