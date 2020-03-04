using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // loads next scene
        // this would actually be after choosing the level etc
        SceneManager.LoadScene("Game Scene");
    }

    public void QuitGame()
    {
        // need to use the oculus one instead that store requires
        Application.Quit();
    }
}
