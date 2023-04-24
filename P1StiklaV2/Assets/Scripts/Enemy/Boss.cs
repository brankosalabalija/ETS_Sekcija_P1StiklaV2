using System.Reflection;
using System.Diagnostics;
using System;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{

    public GameObject Hud;
    public GameObject Health;

    public Transform Player;
    private int health=200;

    public GameObject FireGuy;
    public GameObject FireGuy2;

    public Transform startProjectile;
    public GameObject Projectile;
    public GameObject Warning;
    public GameObject FireColumn;

    Stopwatch reload;
    int reloadTime=5000;

    bool isPhaseOne;
    bool isAnimating;

    Animator _animator;

    Stopwatch shooting;
    Stopwatch summoning;

    void Start()
    {
        _animator=GetComponent<Animator>();
        shooting=new Stopwatch();
        summoning=new Stopwatch();
        reload=new Stopwatch();
        reload.Start();
        isAnimating=false;
        isPhaseOne=true;
    }

    bool isEnd=false;
    void Update()
    {
        
        if(health<=0&&!isEnd)
        {
            Hud.GetComponent<HUDScript>().FadeOUT();
            LoseFire();
            Invoke("GameOver",1f);
            isEnd=true;
        }
        else if(health<=0&&isEnd)
        {
            return;
        }
        if(health<100&&isPhaseOne)
        {
            Invoke("GetFire",1f);
            Instantiate(Warning,FireGuy.transform.position,Quaternion.identity);
            Instantiate(Warning,FireGuy2.transform.position,Quaternion.identity);
            isPhaseOne=false;
            isAnimating=false;
            _animator.SetBool("summon",false);
            _animator.SetBool("shoot",false);
            _animator.SetBool("idle",true);
            _animator.SetBool("phasetwo",true);
            shooting.Reset();
            shooting.Stop();
            summoning.Reset();
            shooting.Stop();
            reload.Start();
            reloadTime/=2;
            UnityEngine.Debug.Log("PhaseTwo");
        }
        

        
        if(shooting.IsRunning||shooting.ElapsedMilliseconds>0)
        {
            if(shooting.ElapsedMilliseconds>4000)
            {   
                _animator.SetBool("shoot",false);
                _animator.SetBool("idle",true);
                shooting.Reset(); 
                shooting.Stop();
                reload.Start();
                isAnimating=false;
            }
            else
            {
                if(Player.position.x<transform.position.x)
                {
                    transform.rotation=Quaternion.Euler(0,180,0);
                }
                else
                {
                    transform.rotation=Quaternion.Euler(0,0,0);
                }
                return;
            }
        }

        if(summoning.IsRunning||summoning.ElapsedMilliseconds>0)
        {
            if(summoning.ElapsedMilliseconds>5000)
            {   
                _animator.SetBool("summon",false);
                _animator.SetBool("idle",true);
                summoning.Reset();
                summoning.Stop();
                reload.Start();
                isAnimating=false;
            }
            else
            {
                return;
            }
        }

        if(reload.IsRunning||reload.ElapsedMilliseconds>0)
        {
            if(reload.ElapsedMilliseconds>reloadTime)
            {
                reload.Reset();
                reload.Stop();
            }
            else
            {
                return;
            }
        }
        
        if(!isAnimating)
        {
            System.Random mechanic=new System.Random();
            mechanic.Next(4);
            switch(mechanic.Next(4))
            {
                case 0:
                    Instantiate(Warning,new Vector2(-17.1f,-5.5f),Quaternion.identity).SetActive(true);
                    reload.Start();
                    Invoke("Strike",1f);
                    break;
                case 1:
                    Instantiate(Warning,new Vector2(3.3f,-5.5f),Quaternion.identity).SetActive(true);
                    reload.Start();
                    Invoke("StrikeR",1f);
                    break;
                case 2:
                    Shoot();
                    break;
                case 3:
                    Summon();
                    break;
            }
        }
        
        
    }
    
    void LoseFire()
    {
        FireGuy.SetActive(false);
        FireGuy2.SetActive(false);
    }
    
    void GetFire()
    {
        FireGuy.SetActive(true);
        FireGuy2.SetActive(true);
    }

    void GameOver()
    {
        //Time.timeScale=0;
        
        SceneManager.LoadScene("end",LoadSceneMode.Single);
        Destroy(this.gameObject);
    }

    private void Strike()
    {
        _animator.SetTrigger("strike");
        
    }


    private void StrikeR()
    {
        _animator.SetTrigger("striker");
        
    }

    private void Summon()
    {
        _animator.SetBool("summon",true);
        _animator.SetBool("idle",false);
        isAnimating=true;
        
    }

    Vector2[] pos;
    int numColumns;
    void SummonFires()
    {
        summoning.Start();

        
        if(isPhaseOne)
            numColumns=UnityEngine.Random.Range(2,5);
        else
            numColumns=UnityEngine.Random.Range(3,7);
        pos=new Vector2[numColumns];
        for(int i=0;i<numColumns;i++)
        {
            pos[i]=new Vector2(UnityEngine.Random.Range(-17,3),-5.1f);
            Instantiate(Warning,pos[i],Quaternion.identity).SetActive(true);
        }
        Invoke("PlaceColumns",1f);
        
    }

    void PlaceColumns()
    {
        for(int i=0;i<numColumns;i++)
        {
            Instantiate(FireColumn,pos[i],Quaternion.identity).SetActive(true);
        }
    }



    private void Shoot()
    {
        _animator.SetBool("shoot",true);
        _animator.SetBool("idle",false);
        shooting.Start();
        isAnimating=true;
        
    }

    private void ShootProjectile()
    {
        GameObject temp=Instantiate(Projectile,startProjectile.position,Quaternion.identity);
        temp.GetComponent<EnemyBeeBullet>().SetDir(Player.transform.position);
        temp.transform.localScale=Vector3.one*3f;
        temp.SetActive(true);
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag=="PlayerProjectile")
        {
            health--;
            Health.GetComponent<HealthBar>().setLives(health);
        }

    }
    
}
