using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScorpionKingScript : MonoBehaviour
{

    public float speed;
    public Vector2 leftPos;
    public Vector2 rightPos;

    Rigidbody2D _rigidbody;

    public int healthPoints;

    Stopwatch _watch;
    private bool canAttack;
    private bool isVisible;

    public GameObject Player;
    public bool isFacingRight;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _watch = new Stopwatch();
        canAttack = true;
        isVisible = false;

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
            _rigidbody.velocity = new Vector2(0, 0);
            isFacingRight = false;
            //Brzina kretanja
            _rigidbody.velocity = (transform.right * speed);
        }
        if (transform.position.x <= leftPos.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _rigidbody.velocity = new Vector2(0, 0);
            isFacingRight = true;
            //Brzina kretanja
            _rigidbody.velocity = (transform.right * speed);
        }



        //unisti ako HP=0 - preko animacije
        if (healthPoints <= 0)
        {
            //dodatni parametri za podesavanje onDeath
            /*speed = 0;
            _rigidbody.gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;*/

            //GetComponent<Animator>().SetTrigger("Death");

            Destroy(this.gameObject);
        }

        //Napada - Udara nogom od pod/zemlju kako bi siljci iskocili. Kad napada onda se ne krece
        if (_watch.ElapsedMilliseconds > 1500) //1500ms = 1.5 seconds
        {
            canAttack = true;
            _rigidbody.velocity = (transform.right * 0); //da se zaustavi dok napada
            GetComponent<Animator>().SetBool("isWalking", false);
        }
        if (canAttack)
        {

            GetComponent<Animator>().SetTrigger("Attack");


            canAttack = false;
            _watch.Restart();
            //Brzina kretanja
            GetComponent<Animator>().SetBool("isWalking", true);
            _rigidbody.velocity = (transform.right * speed);

        }
        //else
        //{
        //    if (!GetComponent<Animator>().GetBool("isWalking"))
        //        GetComponent<Animator>().SetBool("isWalking", true);
        //}
    }

    //Kad pogodi stikla umanji HP
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            healthPoints--;
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
}
