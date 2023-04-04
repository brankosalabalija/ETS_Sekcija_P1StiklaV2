using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string SceneName;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name=="Player")
            SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
    }
}
