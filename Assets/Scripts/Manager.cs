using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public float runEveryXSeconds;
    public Canvas nextLevelCanvas;
    // public Canvas creditsCanvas;
    public Text sceneText;
    public float resuceLevelTime;

    private int currentSceneIndex;
    private int bikeCount = 0;
    private float currentTime = 0;
    private playerHealth player;
    private bool isDead;
    private bool paused = false;

    private bool companionRescued = false;
    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        isDead = false;
        player = FindObjectOfType<playerHealth>();
        if(currentSceneIndex == 1)
            CountInfected();
    }

    private void Update()
    {
        // DESERT SCENE! FIGHTING SCENE. MUST KILL ALL INFECTED TO PROCEED
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

        // RESIDENTIAL SCENE! STEALTH SCENE. MUST FIND 20 BIKES.
        if(currentSceneIndex == 2)
        {
            sceneText.text = "Find all bikes! Bikes left: " + (20 - bikeCount) + " / 20";
            if (bikeCount >= 20)
            {
                nextLevelCanvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                // Time.timeScale = 0;
            }
        }

        // OFFICE SCENE! RESCUE SCENE. MUST RESCUE COMPANION WITHIN TIMELIMIT.
        if(currentSceneIndex == 3)
        {
            if(resuceLevelTime <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                    player.applyDamage(1000);
                }
            }
            else
            {
                sceneText.text = "Rescue companion! Time left: " + (int)resuceLevelTime;
                if(!paused && !companionRescued)
                    resuceLevelTime -= Time.deltaTime;

                if (companionRescued)
                {
                    /*if (CountInfected() == 0)
                    {
                        creditsCanvas.gameObject.SetActive(true);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                    }*/
                }
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

    public void PauseTimer()
    {
        if(currentSceneIndex == 3)
        {
            if (!paused)
                paused = true;
            else
                paused = false;
        }
    }

    public void CompanionRescued()
    {
        if (currentSceneIndex == 3)
        {
            companionRescued = true;
        }
    }
    public void removeBike()
    {
        bikeCount += 1;
    }
}
