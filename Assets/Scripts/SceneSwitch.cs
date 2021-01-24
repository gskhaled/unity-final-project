using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void GoToLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); // rescue level
    }

    public void GoToNextLevel()
    {
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex == 3? 0: currSceneIndex + 1);
    }

    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
