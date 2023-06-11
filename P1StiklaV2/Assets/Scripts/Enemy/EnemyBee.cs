using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBee : MonoBehaviour
{
    public int lives;
    public GameObject BeeBullet;
    public Transform BeeShootingSpot;
    public GameObject Player;

    private Stopwatch _watch;

    private bool canShoot;
    private bool isVisible;
    private bool isFacingRight;


    void Start()
    {
        _watch=new Stopwatch();
        canShoot=true;
    }


    void Update()
    {
        if(!isVisible)
            return;
        if(Player.transform.position.x>transform.position.x)
        {
            transform.rotation=Quaternion.Euler(0,180,0);
            isFacingRight = true;
        }
        else
        {
            transform.rotation=Quaternion.Euler(0,0,0); 
            isFacingRight = false;
        }


        if(_watch.ElapsedMilliseconds>2000)
        {
            canShoot=true;
        }
        if(canShoot)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            GameObject temp=GameObject.Instantiate(BeeBullet,BeeShootingSpot.position,Quaternion.identity);
            //temp.GetComponent<EnemyBeeBullet>().SetDir(Player.transform.position); //100% acc
            if(isFacingRight)
            {
                temp.GetComponent<EnemyBeeBullet>().SetDir(BeeShootingSpot.transform.position + new Vector3(1, -1, 0)); //ugao od 225 stepeni
            }
            else
            {
                temp.GetComponent<EnemyBeeBullet>().SetDir(BeeShootingSpot.transform.position + new Vector3(-1, -1, 0)); //ugao od 315 stepeni
            }
            
            temp.SetActive(true);
            canShoot=false;
            _watch.Restart();
        }
        if(lives==0)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnBecameVisible() {
        _watch.Start();
        isVisible=true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="PlayerProjectile")
        {
            lives--;
            GetComponent<Animator>().SetTrigger("Hurt");
        }
    }

    private void OnBecameInvisible() {
        _watch.Stop();
        isVisible=false;
    }
}
