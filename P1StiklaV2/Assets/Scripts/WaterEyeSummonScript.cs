using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WaterEyeSummonScript : MonoBehaviour
{

    public GameObject player;
    public Transform spawnPos;

    private Vector2 movingDirection;
    private Vector2 thisPos;
    private Stopwatch _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new Stopwatch();
        _timer.Start();
        thisPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if(_timer.ElapsedMilliseconds < 1000)
        {
            movingDirection = player.transform.position;
            return;
        }
        else if (_timer.ElapsedMilliseconds > 10000)
        {
            _timer.Stop();
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = (movingDirection - thisPos).normalized * 8f;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != 7)
            Destroy(this.gameObject);
    }
}
