using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject HUD;
    public string SceneName;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name=="Player")
        {
            HUD.GetComponent<HUDScript>().FadeOUT();
            Invoke("ChangeScene",1f);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
    }
}
