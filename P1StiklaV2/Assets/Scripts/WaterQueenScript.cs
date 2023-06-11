using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WaterQueenScript : MonoBehaviour
{
    public int healthPoints;

    public Transform bulletPos;
    public GameObject waterBullet;

    public Transform eyeSummonPos;
    public GameObject eyeSummon;

    public GameObject player;
    public Vector2 leftPos;
    public Vector2 rightPos;
    public int speed;



    private bool isVisible;
    private bool isFacingRight;
    private bool canShoot;
    private Stopwatch _watch;
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        isVisible = false;
        _watch = new Stopwatch();
        isFacingRight = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isVisible)
            return;

        //Kretanje i okretanje u odnosu na poziciju po x-osi sa rotaciom
        if (transform.position.x >= rightPos.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _rigidbody2D.velocity = new Vector2(0, 0);
            isFacingRight = false;
            //Brzina kretanja
            _rigidbody2D.velocity = (transform.right * speed);
        }
        if (transform.position.x <= leftPos.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _rigidbody2D.velocity = new Vector2(0, 0);
            isFacingRight = true;
            //Brzina kretanja
            _rigidbody2D.velocity = (transform.right * speed);
        }

        if (_watch.ElapsedMilliseconds > 2000)
        {
            canShoot = true;

        }

        if (canShoot)
        {
            _rigidbody2D.velocity = new Vector2(0, 0); //zaustavi se, ispali projektil

            if (Random.Range(0, 10) < 8)// ako je Random 0,1,2,3,4,5,6,7 ispali WaterBullet
            {
                
                GetComponent<Animator>().SetTrigger("shoot");

                GameObject tempBullet = GameObject.Instantiate(waterBullet, bulletPos.position, Quaternion.identity);
                if (isFacingRight)
                {
                    tempBullet.GetComponent<WaterBulletScript>().SetDir(bulletPos.transform.position + new Vector3(1, 0, 0));// 0 stepeni
                }
                else
                {
                    tempBullet.GetComponent<WaterBulletScript>().SetDir(bulletPos.transform.position + new Vector3(-1, 0, 0));// 180 stepeni
                }

                tempBullet.SetActive(true);
                canShoot = false;
                _watch.Restart();
                GetComponent<Animator>().SetTrigger("walk");
                
            }
            else //ako je Random 8,9 ispali-prizovi WaterEyeBullet
            {
                GetComponent<Animator>().SetTrigger("summon");
                GameObject tempEye = GameObject.Instantiate(eyeSummon, eyeSummonPos.position, Quaternion.identity);
                tempEye.SetActive(true);

                canShoot = false;
                _watch.Restart();
                GetComponent<Animator>().SetTrigger("walk");
                _rigidbody2D.velocity = (transform.right * speed);// pa nastavi se kretati

                //if (_watch.ElapsedMilliseconds > 700)// cekaj 0.7 pa onda se nastavi kretati
                //{
                //    _watch.Restart();
                //}
            }
        }

        if (healthPoints <= 0)
        {
            Destroy(this);
        }
    }

    private void OnBecameInvisible()
    {
        _watch.Stop();
        isVisible = false;
    }

    private void OnBecameVisible()
    {
        _watch.Start();
        isVisible = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            healthPoints--;
        }
    }

}
