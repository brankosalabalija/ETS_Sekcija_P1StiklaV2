using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScavangerEnemie : MonoBehaviour
{
    public int healthPoints;
    public Transform bulletPos;
    public GameObject scavBullet;
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
        isFacingRight = true;
        
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
            _rigidbody2D.velocity = Vector2.zero; //zaustavi se, ispali projektil
            GetComponent<Animator>().SetBool("IsShooting", true); 
            GetComponent<Animator>().SetBool("IsWalking", false);

            GameObject temp = GameObject.Instantiate(scavBullet, bulletPos.position , Quaternion.identity);
            if (isFacingRight)
            {
                temp.GetComponent<ScavBulletScript>().SetDir(bulletPos.transform.position + new Vector3(1, 0, 0));// 0 stepeni
                temp.GetComponent<SpriteRenderer>().flipX = !isFacingRight;

            }
            else
            {
                temp.GetComponent<ScavBulletScript>().SetDir(bulletPos.transform.position + new Vector3(-1, 0, 0));// 180 stepeni
                temp.GetComponent<SpriteRenderer>().flipX = !isFacingRight;
            }

            temp.SetActive(true);
            canShoot = false;
            _watch.Restart();
            GetComponent<Animator>().SetBool("IsShooting", false);
            GetComponent<Animator>().SetBool("IsWalking", true);
            _rigidbody2D.velocity = (transform.right * speed);

        }

        if (healthPoints <= 0)
        {
            Destroy(this.gameObject);
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
