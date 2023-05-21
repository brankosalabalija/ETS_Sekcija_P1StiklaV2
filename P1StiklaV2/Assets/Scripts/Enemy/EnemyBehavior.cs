using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed;
    public Vector2 leftPos;
    public Vector2 rightPos;
    Rigidbody2D _rigidbody;
    private int Lives;


    void Start()
    {
        Lives=3;
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
