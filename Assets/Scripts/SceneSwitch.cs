using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void GoToLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Scenes/OfficeScene_test");
    }

    public void GoToLevel2()
    {
        Time.timeScale = 1;
    }

    public void GoToLevel3()
    {
        Time.timeScale = 1;
    }

    public void GotoMainMenuScene()
    {
        SceneManager.LoadScene("Scenes/Menus Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
