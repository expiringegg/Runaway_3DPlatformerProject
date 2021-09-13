using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public void ChangetheScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ReloadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayAllow()
    {
        Time.timeScale = 1f;
    }

}