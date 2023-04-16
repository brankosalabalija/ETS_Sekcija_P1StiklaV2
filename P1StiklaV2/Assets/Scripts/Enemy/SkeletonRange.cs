using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonRange : MonoBehaviour
{
    int lives=3;
    public GameObject bulletPOS;
    Animator _animator;
    Rigidbody2D _rigidbody;
    public GameObject bullet;

    public Vector2 leftPos;
    public Vector2 rightPos;

    Stopwatch reload;

    void Start()
    {
        _animator=GetComponent<Animator>();
        _rigidbody=GetComponent<Rigidbody2D>();
        reload=new Stopwatch();
    }

    void Update()
    {
        if(!reload.IsRunning)
        {
            reload.Start();
        }
        if(!isPlayerFarAway())
        {
            if(reload.ElapsedMilliseconds>1500)
            {
                _rigidbody.velocity=new Vector2(0,0);
                _animator.SetBool("Stay",true);
                if(GameObject.Find("Player").transform.position.x<=transform.position.x)
                    transform.rotation=Quaternion.Euler(0,180,0);
                else
                    transform.rotation=Quaternion.Euler(0,0,0);
                reload.Restart();
            }
        }
        else 
        {
            _animator.SetBool("Stay",false);
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
            reload.Stop();
            reload.Reset();
            _rigidbody.velocity=(transform.right*1.1f);
        }
        if(lives<=0)
        {
            Death();
        }

    }

    public void Shoot()
    {
        GameObject a = Instantiate(bullet,bulletPOS.transform.position,Quaternion.identity);
        a.AddComponent<Rigidbody2D>();
        a.transform.right=transform.right;
        a.SetActive(true);
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.tag=="PlayerProjectile")
        {
            lives--;
        }
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    bool isPlayerFarAway()
    {
        return Vector2.Distance(this.transform.position,GameObject.Find("Player").transform.position)>15;
    }

}
