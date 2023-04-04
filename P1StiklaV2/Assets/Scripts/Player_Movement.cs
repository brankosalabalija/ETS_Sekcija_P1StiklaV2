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

    public float moveSpeed = 9f;    // Brzina kretanja
    public float jumpSpeed = 19f;   // Brzina skoka
    public float fallSpeed = 4f;    // Brzina pada/gravitacija za igraca
    public float crouchSpeed = 1f;  // Brzina pada/gravitacija za igraca

    private bool canJump;            // Pokazuje da li moze da skace igrac
    private bool onGround;          // Pokazuje da li je na zemlji
    private bool onLadder;          // Pokazuje da li je na ljestvama
    private bool inLadderTrigger;   // Pokazuje da li je u triggeru ljestvama
    private bool isCrouching;       // Pokazuje da li igrac cuci
    

    private float normalHeight;     // Normalna visina igraca
    private float crouchHeight;     // Crouch visina igraca

    public GameObject bulletPrefab; // Objekat kojim se instacira metak
    static int allowedThrows = 3;   // Dozvoljen broj metaka
    public int facing = 0;          // 0-Desno 1-Lijevo
    public static int bulletDirection = 0; // Smjer metka [0-5]
    
    private Stopwatch jumpTimer;    // Kada sidje sa platforme ima malo vremena da skoci
    
    
    
    
    void Start()
    {
        //Vrijednosti brzina
        moveSpeed = 9f;
        jumpSpeed = 19f;
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
        
        
    }

    void Update()
    {

        ///Osnovne kontrole igraca
        
        //Uzima unos igraca (lijevo,desno,gore,dole)
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        //Facing
        if (dirX > 0)
            facing = 0;
        else if (dirX < 0)
            facing = 1;
        
        //Da li je na zemlji igrac
        onGround = GroundCheck();
        
        //Kretanje na x osi (lijevo,desno)
        if(!isCrouching)
            rb.velocity = new Vector2((moveSpeed * dirX), rb.velocity.y);
        else if(isCrouching)
            rb.velocity = new Vector2((crouchSpeed * dirX), rb.velocity.y);


        //Skok igraca
        
        if (Input.GetButtonDown("Jump")&&canJump&&!isCrouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        
        if(onGround&&!isCrouching&&!canJump) //Provjerava da li igrac moze da skoci
        {
            canJump=true;
        }
        else if(!onGround&&canJump) // Ako sidje sa platforme ima 0.050 da skoci opet
        {
            canJump=false;
            
            if(!jumpTimer.IsRunning)
            {
                jumpTimer.Start();
            }
            else if(jumpTimer.ElapsedMilliseconds>50)
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
        
        
        
        
        //Cucanj igraca
        if (onGround&&dirY<0)
        {
            //Trazi podlogu da popravi poziciju nakon smanjenja igraca
            RaycastHit2D groundSearch = Physics2D.BoxCast(colliders.bounds.center, colliders.bounds.size, 0f, Vector2.down, .1f, GroundLayer);
            transform.localScale = new Vector2(transform.localScale.x, crouchHeight); //Smanjuje igraca
            transform.localPosition = new Vector2(transform.localPosition.x,groundSearch.point.y+crouchHeight/2); //
            isCrouching = true;
        }
        else if(isCrouching)
        {
            //Trazi podlogu da popravi poziciju nakon smanjenja hitboxa
            RaycastHit2D groundSearch = Physics2D.BoxCast(colliders.bounds.center, colliders.bounds.size, 0f, Vector2.down, .1f, GroundLayer);
            transform.localScale = new Vector2(transform.localScale.x, normalHeight); //Povecava igraca na org vrijednost
            transform.localPosition = new Vector2(transform.localPosition.x, groundSearch.point.y + normalHeight / 2);
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
        if (Input.GetButtonDown("Fire1"))
        {
            if(allowedThrows>0)
            {
                Vector3 pos;
                if (facing==1) //Ako gleda lijevo stavi poziciju stvaranja na lijevu ivicu igraca
                    pos = new Vector3(colliders.bounds.center.x-colliders.bounds.size.x/2-0.3f,colliders.bounds.center.y);
                else //Ako gleda desno stavi poziciju stvaranja na desnu ivicu igraca
                    pos = new Vector3(colliders.bounds.center.x + colliders.bounds.size.x / 2+0.3f, colliders.bounds.center.y);
                
                // Smjer metaka
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
                //Instanciranje objekta
                GameObject a = Instantiate(bulletPrefab, pos, Quaternion.identity);
                a.SetActive(true); // Aktiviranje objekta
                allowedThrows--; // Smanjuje broj dozvoljenih metaka
            }
            
        }
        

    }

    //Provjerava da li je igrac na zemlji
    private BoxCollider2D colliders;
    bool GroundCheck()
    { 
        return Physics2D.BoxCast(colliders.bounds.center, colliders.bounds.size, 0f, Vector2.down, .1f, GroundLayer);
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
            PlayerDeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag=="Death") 
        {
            PlayerDeath();
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
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.buildIndex,LoadSceneMode.Single);
    }
    
}
