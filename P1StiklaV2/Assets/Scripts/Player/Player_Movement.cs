using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{

    [SerializeField] private LayerMask GroundLayer; //Koristi se za groundCheck()
    private Rigidbody2D rb;

    public GameObject _HUD;
    public HUDScript _HUDScript;

     float dirX;
     float dirY;

    public int lives;

    public float moveSpeed = 9f;    // Brzina kretanja
    public float jumpSpeed = 19f;   // Brzina skoka
    public float fallSpeed = 4f;    // Brzina pada/gravitacija za igraca
    public float crouchSpeed = 1f;  // Brzina pada/gravitacija za igraca

    private bool canJump;            // Pokazuje da li moze da skace igrac           
    private bool canMove;            
    public bool onGround;          // Pokazuje da li je na zemlji
    private bool onLadder;          // Pokazuje da li je na ljestvama
    private bool inLadderTrigger;   // Pokazuje da li je u triggeru ljestvama
    private bool isCrouching;       // Pokazuje da li igrac cuci
    

    private float normalHeight;     // Normalna visina igraca
    private float crouchHeight;     // Crouch visina igraca

    public Transform bulletPosition;
    public GameObject bulletPrefab; // Objekat kojim se instacira metak
    static int allowedThrows = 2;   // Dozvoljen broj metaka
    public int facing = 0;          // 0-Desno 1-Lijevo
    public static int bulletDirection = 0; // Smjer metka [0-5]
    
    private Stopwatch jumpTimer;    // Kada sidje sa platforme ima malo vremena da skoci
    private Stopwatch invinceFrames;    // Kada sidje sa platforme ima malo vremena da skoci
    
    Animator _animator;
    
    void Start()
    {
        _HUDScript=_HUD.GetComponent<HUDScript>();
        lives=3;
        //Vrijednosti brzina
        moveSpeed = 9f;
        jumpSpeed = 15f;
        fallSpeed = 4f;
        crouchSpeed = 1f;
        
        //Za kontrolu rigidbody-a
        rb = GetComponent<Rigidbody2D>(); 
        rb.gravityScale = fallSpeed;
        rb.freezeRotation = true;

        //Za kontrolu nad BoxColliderom kod groundCheck()
        colliders = GetComponent<BoxCollider2D>();
        
        //Uzima visinu igraca i visinu kada cuci
        normalHeight = transform.localScale.y;
        crouchHeight = normalHeight * 0.65f; 
        
        jumpTimer=new Stopwatch();
        invinceFrames=new Stopwatch();
        
        _animator=GetComponentInChildren<Animator>();
        _animator.gameObject.SetActive(true);
        
        canMove=true;
        
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            _HUDScript.Pause();
        }
        ///Osnovne kontrole igraca
        
        //Uzima unos igraca (lijevo,desno,gore,dole)
        if(canMove)
        {
        dirX = Input.GetAxisRaw("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");

        }
        
        

        //Facing
        if (dirX > 0)
        {
            facing = 0;
            transform.rotation=Quaternion.Euler(0,0,0);
        }
        else if (dirX < 0)
        {
            facing = 1;
            transform.rotation=Quaternion.Euler(0,180,0);
        }   
        if(dirX!=0)
        {
            _animator.SetBool("Running",true);
        }
        else
        {
            _animator.SetBool("Running",false);
        }
        
        //Da li je na zemlji igrac
        //onGround = GroundCheck();
        
        //Kretanje na x osi (lijevo,desno)
        if(!isCrouching)
            rb.velocity = new Vector2((moveSpeed * dirX), rb.velocity.y);
        else if(isCrouching)
            rb.velocity = new Vector2((crouchSpeed * dirX), rb.velocity.y);


        //Skok igraca
        if (Input.GetButton("Jump")&&canJump&&!isCrouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        
        if(onGround&&!isCrouching) //Provjerava da li igrac moze da skoci
        {
            canJump=true;
        }
        else if(!onGround&&canJump) // Ako sidje sa platforme ima 0.050 da skoci opet
        {
            //canJump=false;
            
            if(!jumpTimer.IsRunning)
            {
                jumpTimer.Start();
            }
            else if(jumpTimer.ElapsedMilliseconds>100)
            {
                canJump=false;
                jumpTimer.Reset();
                jumpTimer.Stop();
                
            }
             
        }
        if(jumpTimer.ElapsedMilliseconds>1000)
        {
            jumpTimer.Reset();
            jumpTimer.Stop();
        }
        
        
        if(invinceFrames.ElapsedMilliseconds>1500&&invinceFrames.IsRunning)
        {
            invinceFrames.Stop();
            invinceFrames.Reset();
            
        }
        
        //Cucanj igraca
        if (onGround&&dirY<0)
        {
            //Trazi podlogu da popravi poziciju nakon smanjenja igraca

            RaycastHit2D groundSearch = Physics2D.BoxCast(colliders.bounds.center, new Vector2(colliders.bounds.size.x/3,colliders.bounds.size.y), 0f, Vector2.down, .01f, GroundLayer);
            transform.localScale = new Vector2(transform.localScale.x, crouchHeight); //Smanjuje igraca
            isCrouching = true;
        }
        else if(isCrouching&&!(dirY<0))
        {
            //Trazi podlogu da popravi poziciju nakon smanjenja hitboxa
            RaycastHit2D groundSearch = Physics2D.BoxCast(colliders.bounds.center, new Vector2(colliders.bounds.size.x/3,colliders.bounds.size.y), 0f, Vector2.down, .01f, GroundLayer);
            transform.localScale = new Vector2(transform.localScale.x, normalHeight); //Povecava igraca na org vrijednost
            isCrouching = false;
        }

        /// Kretanje igraca na ljestvama
        // Kad udje u trigger ceka unos igraca na y osi tako da bih ga stavio u stanje "na ljestvama" 
        if (inLadderTrigger && dirY != 0) 
        {
            onLadder = true;
        }
        if (onLadder)
        {
            //stavlja se gravitacija 0 da igrac ne bi usporeno padao na ljestvama
            rb.gravityScale = 0; 
            rb.velocity = new Vector2(rb.velocity.x, ((moveSpeed-2) * dirY)); 
        }


        
        
        // Pucanje/Bacanje oruzja
        if (Input.GetButtonDown("Fire1")&&allowedThrows>0)
        {
            _animator.SetTrigger("Throwing");
            
            if(allowedThrows>0)
            {
                if(dirY<0) // Dole
                {
                    if (facing == 0) //Dole lijevo
                        bulletDirection = 2;
                    else //Dole desno
                        bulletDirection = 5;
                    
                }
                else if(dirY>0) // Gore
                {
                    if(facing==0) //Gore desno
                        bulletDirection = 0;
                    else //Gore lijevo
                        bulletDirection = 4;
                }
                else if(dirY==0) // Ravno
                {
                    if (facing == 0) // Desno
                        bulletDirection = 1; 
                    else // Lijevo
                        bulletDirection = 3;
                }
                }

                //Instanciranje objekta
                GameObject a = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
                a.SetActive(true); // Aktiviranje objekta
                allowedThrows--; // Smanjuje broj dozvoljenih metaka
        }
        

    }

    private void Hurt()
    {
        lives--;
        if(lives==0)
        {
            
            PlayerDeath();
        }
        else
        {
            _animator.SetTrigger("Hurt");
            if (lives <= 3)
            {
                _HUDScript.UILoseLife();
            }
            invinceFrames.Restart();
        }
    }

    //Provjerava da li je igrac na zemlji
    private BoxCollider2D colliders;
    bool GroundCheck()
    { 
        Vector2 size=new Vector2(colliders.bounds.size.x/3,colliders.bounds.size.y);
        return Physics2D.BoxCast(colliders.bounds.center, size, 0f, Vector2.down, .15f, GroundLayer);
    }

    /// Triggeri
    private void OnTriggerEnter2D(Collider2D other)
    {
        string triggerName = other.name.ToUpper();
        if (triggerName == "LADDER")
        {
            inLadderTrigger = true;
        }
        if (other.gameObject.tag=="Death") 
        {
            if(other.gameObject.name=="DeadZone")
                PlayerDeath();
            else if(!invinceFrames.IsRunning)
            {
                invinceFrames.Start();
                Hurt();
            }
        }
    }


    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.tag=="Death")
        {
            if(!(invinceFrames.ElapsedMilliseconds>0))
                Hurt();

            
        }
        if(other.gameObject.layer==LayerMask.NameToLayer("Ground"))
        {
            onGround=GroundCheck();
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.layer==LayerMask.NameToLayer("Ground"))
        {
            onGround=false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag=="Death") 
        {
            if(!invinceFrames.IsRunning)
            {
                invinceFrames.Start();
                Hurt();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Ladder")
        {
            inLadderTrigger = false;
            onLadder = false;
            rb.gravityScale = fallSpeed;
        }
    }

    // Da ostale skripte mogu dobiti facing igraca
    public static int GetDirection()
    {
        return bulletDirection;
    }

    // Kada se metak unisti ova funkcija se poziva tako da se poveca broj dozvoljenih metaka
    public static void BulletDestroy()
    {
        allowedThrows++;
    }

    public void PlayerDeath()
    {
        canMove=false;
        _HUDScript.FadeOUT();
        
        Invoke("SceneReset",1.1f);
        
    }

    private void SceneReset()
    {
        _HUDScript.UIReset();
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.buildIndex,LoadSceneMode.Single);
    }
    
}
