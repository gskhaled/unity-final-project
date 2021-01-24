using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0 && PlayerPrefs.GetString("Credits") != null && PlayerPrefs.GetString("Credits").Equals("Yes"))
        {
            GameObject mainMenuCanvas = GameObject.Find("Main Menu Canvas");
            //mainMenuCanvas.transform.GetChild(0).gameObject.SetActive(false);
            mainMenuCanvas.transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    public void ResetCredits()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlayerPrefs.SetString("Credits", "No");
        }
    }

    public void GoToLevel1(string companionName)
    {
        PlayerPrefs.SetString("Companion", companionName);
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
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
