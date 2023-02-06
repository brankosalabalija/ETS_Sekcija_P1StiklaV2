using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{

    [SerializeField] private LayerMask GroundLayer; //Koristi se za groundCheck()

    private Rigidbody2D rb;

    public float moveSpeed; //Brzina kretanja
    public float jumpSpeed; //Brzina skoka
    public float fallSpeed; //Brzina pada/gravitacija za igraca
    public float crouchSpeed; //Brzina pada/gravitacija za igraca

    private bool onGround; //Pokazuje da li je na zemlji
    private bool onLadder; //Pokazuje da li je na ljestvama
    private bool inLadderTrigger; //Pokazuje da li je u triggeru ljestvama
    private bool isCrouching; //Pokazuje da li igrac cuci

    private float normalHeight;
    private float crouchHeight;

    void Start()
    {
        //Vrijednosti brzina
        moveSpeed = 7f;
        jumpSpeed = 14f;
        fallSpeed = 4f;
        crouchSpeed = 0f;

        //Za kontrolu rigidbody-a
        rb = GetComponent<Rigidbody2D>(); 
        rb.gravityScale = fallSpeed;
        rb.freezeRotation = true;

        //Za kontrolu nad BoxColliderom kod groundCheck()
        colliders = GetComponent<BoxCollider2D>();

        normalHeight = transform.localScale.y;
        crouchHeight = normalHeight * 0.65f;
    }

    void Update()
    {

        ///Osnovne kontrole igraca
        
        //Uzima unos igraca (lijevo,desno,gore,dole)
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        //Da li je na zemlji igrac
        onGround = groundCheck();

        //Kretanje na x osi (lijevo,desno)
        if(!isCrouching)
            rb.velocity = new Vector2((moveSpeed * dirX), rb.velocity.y);
        else if(isCrouching)
            rb.velocity = new Vector2((crouchSpeed * dirX), rb.velocity.y);

        //Skok igraca
        if (Input.GetButtonDown("Jump") && onGround && !isCrouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
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
        
    }

    //Provjerava da li je igrac na zemlji
    private BoxCollider2D colliders;
    bool groundCheck()
    { 
        return Physics2D.BoxCast(colliders.bounds.center, colliders.bounds.size, 0f, Vector2.down, .1f, GroundLayer);
    }    
    
    /// Igrac na ljestvama
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Ladder")
        {
            inLadderTrigger = true;
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
}
