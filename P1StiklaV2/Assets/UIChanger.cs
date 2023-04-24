using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIChanger : MonoBehaviour
{
    public string scene;
    public void ChangeScene(string Scene)
    {
        SceneManager.LoadScene(Scene,LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
