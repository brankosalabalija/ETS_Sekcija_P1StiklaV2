using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public int speed;
    public Vector2 rightPos;
    public Vector2 leftPos;
    Rigidbody2D _rigidbody;
    void Start()
    {
        _rigidbody=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(transform.position.x>=rightPos.x)
        {
            transform.rotation=Quaternion.Euler(0,180,0);
            _rigidbody.velocity=new Vector2(0,0);
        }
        if(transform.position.x<=leftPos.x)
        {
            transform.rotation=Quaternion.Euler(0,0,0);
            _rigidbody.velocity=new Vector2(0,0);
        }

        _rigidbody.velocity=(transform.right*speed);
    }
}
