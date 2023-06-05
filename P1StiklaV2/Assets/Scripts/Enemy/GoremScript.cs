using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoremScript : MonoBehaviour
{
    public float speed;
    public Vector2 leftPos;
    public Vector2 rightPos;
    public Vector2 spikeOff;
    private Vector3 spikeOffV3;
    
    Rigidbody2D _rigidbody;
    
    public int healtPoints;
    
    Stopwatch _watch;
    private bool canAttack;

    public GameObject gSpike;
    public Transform spikeSpawnPos;
    public GameObject Player;
    public bool isFacingRight;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _watch = new Stopwatch();
        canAttack = true;
        spikeOffV3 = spikeOff;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (healtPoints <= 0)
        {
            //dodatni parametri za podešavanje onDeath
            /*speed = 0;
            _rigidbody.gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;*/
            
            GetComponent<Animator>().SetTrigger("Death");

            //Destroy(this.gameObject);
        }

        //Napada - Udara nogom od pod/zemlju kako bi šiljci iskočili. Kad napada onda se ne kreće
        if (_watch.ElapsedMilliseconds > 2000)
        {
            canAttack = true;
            _rigidbody.velocity = Vector2.zero; //da se zaustavi dok napada
        }
        if (canAttack)
        {
            

            GetComponent<Animator>().SetTrigger("Stomp");

            GameObject temp = GameObject.Instantiate(gSpike, (spikeSpawnPos.position + spikeOffV3), Quaternion.identity);

            temp.SetActive(true);
            canAttack = false;
            _watch.Restart();
            //Brzina kretanja
            _rigidbody.velocity = (transform.right * speed);
        }
    }

    //Kad pogodi stikla umanji HP
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            healtPoints--;
        }
    }
}
