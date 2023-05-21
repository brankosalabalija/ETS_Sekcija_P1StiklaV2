using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoremScript : MonoBehaviour
{
    public float speed;
    public Vector2 leftPos;
    public Vector2 rightPos;
    
    Rigidbody2D _rigidbody;
    
    public int healtPoints;
    
    Stopwatch _watch;
    private bool canAttack;

    public GameObject gSpike;
    public Transform spikeSpawnPos;
    public GameObject Player;
    private bool isFacingRight;


    // Start is called before the first frame update
    void Start()
    {
        
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
        }
        if (transform.position.x <= leftPos.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _rigidbody.velocity = new Vector2(0, 0);
            isFacingRight = true;
        }

        //Brzina kretanja
        _rigidbody.velocity = (transform.right * speed);

        //unisti ako HP=0
        if (healtPoints <= 0)
        {
            Destroy(this.gameObject);
        }


        if (_watch.ElapsedMilliseconds > 2000)
        {
            canAttack = true;
        }
        if (canAttack)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            GameObject temp = GameObject.Instantiate(gSpike, spikeSpawnPos.position, Quaternion.identity);
            if (isFacingRight)
            {
                //temp.GetComponent<EnemyBeeBullet>().SetDir(spikeSpawnPos.transform.position + new Vector3(1, -1, 0)); 
            }
            else
            {
                //temp.GetComponent<EnemyBeeBullet>().SetDir(spikeSpawnPos.transform.position + new Vector3(-1, -1, 0)); 
            }

            temp.SetActive(true);
            canAttack = false;
            _watch.Restart();
        }
    }

    //Kad pogodi stikla umanji HP
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            healtPoints--;
        }
    }
}
