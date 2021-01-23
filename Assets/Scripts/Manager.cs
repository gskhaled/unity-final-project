using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public float runEveryXSeconds;
    public Canvas nextLevelCanvas;
    public Text sceneText;
    public float resuceLevelTime;

    private int currentSceneIndex;
    private int bikeCount = 0;
    private float currentTime = 0;
    private playerHealth player;
    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        player = FindObjectOfType<playerHealth>();
        if(currentSceneIndex == 1)
            CountInfected();
    }

    private void Update()
    {
        if(currentSceneIndex == 1)
        {
            if(currentTime < runEveryXSeconds)
            {
                currentTime = currentTime + Time.deltaTime;
            }
            else
            {
                if (CountInfected() == 0)
                {
                    nextLevelCanvas.gameObject.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    // Time.timeScale = 0;
                }
                else
                    currentTime = 0;
            }
        }

        if(currentSceneIndex == 2)
        {
            sceneText.text = "Find all bikes! Bikes left: " + bikeCount + " / 20";
            if (bikeCount >= 20)
            {
                nextLevelCanvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // Time.timeScale = 0;
            }
        }

        if(currentSceneIndex == 3)
        {
            if(resuceLevelTime <= 0)
            {
                player.applyDamage(1000);
            }
            else
            {
                sceneText.text = "Rescue companion! Time left: " + (int)resuceLevelTime;
                resuceLevelTime = resuceLevelTime - Time.deltaTime;
            }
        }
    }

    public void CollectBike()
    {
        if (currentSceneIndex == 2)
            bikeCount += 1;
    }

    private int CountInfected()
    {
        NormalLogic[] normalInfected = FindObjectsOfType<NormalLogic>();
        HunterLogic[] hunterInfected = FindObjectsOfType<HunterLogic>();
        ChargerLogic[] chargerInfected = FindObjectsOfType<ChargerLogic>();
        TankLogic[] tankInfected = FindObjectsOfType<TankLogic>();
        SpitterLogic[] spitterInfected = FindObjectsOfType<SpitterLogic>();
        int count = spitterInfected.Length + tankInfected.Length + chargerInfected.Length + hunterInfected.Length + normalInfected.Length;
        sceneText.text = "Kill all infected! Infected Left: " + count;
        return count;
    }
}
