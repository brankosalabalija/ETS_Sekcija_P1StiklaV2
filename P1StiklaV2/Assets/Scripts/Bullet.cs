using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 bulletSpeed = new Vector2(25, 8.5f); // Brzina metka
    public int direction = 1;

    /// Smjerovi metka
    /// 4   0
    /// 3 P 1
    /// 5   2
    
    Rigidbody2D rb = new Rigidbody2D();
    Collider2D col = new Collider2D();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //Destroy(this.gameObject,3); // Brise Metak poslije 5 sekundi ako ne udari nesto
        direction = Player_Movement.GetDirection(); // Uzima smjer
        switch(direction)
        {
            case 0:
                bulletSpeed = new Vector2(bulletSpeed.x, bulletSpeed.y+10);
                break;
            case 1:
                break;
            case 2:
                bulletSpeed = new Vector2(bulletSpeed.x, bulletSpeed.y - 15);
                break;
            case 3:
                bulletSpeed = new Vector2(-bulletSpeed.x, bulletSpeed.y);
                break;
            case 4:
                bulletSpeed = new Vector2(-bulletSpeed.x, bulletSpeed.y + 10);
                break;
            case 5:
                bulletSpeed = new Vector2(-bulletSpeed.x, bulletSpeed.y - 15);
                break;
        }
        rb.gravityScale = 3; // Gravitacija metka
        rb.velocity= bulletSpeed; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Ako udari nesto onda se brise
        if (collision.collider.name != "Player")
        {
            Destroy(this.gameObject);
        }
            
    }

    private void OnBecameInvisible() //Ako izadje sa kamere
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        Player_Movement.BulletDestroy();
    }

}
