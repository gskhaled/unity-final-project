using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public playerHealth healthAndRageScript;
    public Scrollbar healthBar;
    public Material healthBarMaterial;
    public Text healthText;
    public Text ARCurrAmo, ARAllAmo, HRCurrAmo, HRAllAmo, PistolCurrAmo, SGCurrAmo, SGAllAmo, SMGCurrAmo, SMGAllAmo;
    public Text louisAmmo, louisMaxAmmo, ellieAmmo, ellieMaxAmmo;
    public GameObject AR, HR, Pistol, SG, SMG, Grenades, EllieAmmoCount, LouisAmmoCount;
    public WeaponSwitching WeaponSwitchingScript;
    private CompanionGun CompanionGunScript;
    public BombThrower BombThrowerScript;
    public Text BombName;
    public Canvas pauseCanvas;
    public Canvas gameOverCanvas;
    public Text levelName;

    public Scrollbar rageBar;
    public Text rageText;
    public Image fullRageImage;
    public Material rageMaterial;

    private int myRage = 100;
    private int myHealth = 0;
    private Gun gun;
    private string bomb;

    void Start()
    { 
        healthBar.size = 1.0f;
        healthBarMaterial.color = new Color(1, 0, 0, 1);
        CompanionGunScript = FindObjectOfType<CompanionGun>();
        if(PlayerPrefs.GetString("Companion").Equals("Ellie"))
        {
            EllieAmmoCount.SetActive(true);
        }  
        if (PlayerPrefs.GetString("Companion").Equals("Louis"))
        {
            LouisAmmoCount.SetActive(true);
        }
    }
    
    void Update()
    {
        louisAmmo.text = "" + CompanionGunScript.getAmmoCount();
        ellieAmmo.text = "" + CompanionGunScript.getAmmoCount();
        louisMaxAmmo.text = "/" + CompanionGunScript.getMaxAmmo();
        ellieMaxAmmo.text = "/" + CompanionGunScript.getMaxAmmo();

        if (WeaponSwitchingScript.getCurrentGun() != null)
        {
            gun = WeaponSwitchingScript.getCurrentGun();
            string gunName = gun.gameObject.name;
            switch (gunName)
            {
                case "AR":
                    ARCurrAmo.text = "" + gun.currentAmmo;
                    ARAllAmo.text = "/" + gun.allAmmo;
                    AR.SetActive(true);
                    HR.SetActive(false);
                    Pistol.SetActive(false);
                    SG.SetActive(false);
                    SMG.SetActive(false);
                    break;

                case "Hunting Rifle":
                    HRCurrAmo.text = "" + gun.currentAmmo;
                    HRAllAmo.text = "/" + gun.allAmmo;
                    AR.SetActive(false);
                    HR.SetActive(true);
                    Pistol.SetActive(false);
                    SG.SetActive(false);
                    SMG.SetActive(false);
                    break;

                case "Pistol":
                    PistolCurrAmo.text = "" + gun.currentAmmo;
                    AR.SetActive(false);
                    HR.SetActive(false);
                    Pistol.SetActive(true);
                    SG.SetActive(false);
                    SMG.SetActive(false);
                    break;

                case "Shotgun":
                    SGCurrAmo.text = "" + gun.currentAmmo;
                    SGAllAmo.text = "/" + gun.allAmmo;
                    AR.SetActive(false);
                    HR.SetActive(false);
                    Pistol.SetActive(false);
                    SG.SetActive(true);
                    SMG.SetActive(false);
                    break;

                case "SMG":
                    SMGCurrAmo.text = "" + gun.currentAmmo;
                    SMGAllAmo.text = "/" + gun.allAmmo;
                    AR.SetActive(false);
                    HR.SetActive(false);
                    Pistol.SetActive(false);
                    SG.SetActive(false);
                    SMG.SetActive(true);
                    break;
            }
        }

        bomb = BombThrowerScript.getCurrentGrenade();
        if (bomb != "Nothing")
        {
            Grenades.SetActive(true);
            switch (bomb)
            {
                case "Molotov":
                    BombName.text = "Molotov Cocktail";
                    break;
                case "StunGrenade":
                    BombName.text = "Stun Grenade";
                    break;
                case "PipeBomb":
                    BombName.text = "Pipe Bomb";
                    break;
            }
        }
        else
            Grenades.SetActive(false);

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    void FixedUpdate()
    {
        health();
        Rage();
    }

    void health()
    {
        myHealth = healthAndRageScript.healthValue();
        healthText.text = "" + myHealth;
        healthBar.size = (float)myHealth / 300;
        if (healthBar.size <= 0.35)
        {
            healthBarMaterial.color = new Color(1, 0, 0, 1);
        }
        else
            healthBarMaterial.color = new Color(0, 1, 0, 1);
        if (myHealth <= 0)
            GameOver();
    }

    void Rage()
    {
        myRage = healthAndRageScript.rageMeterNumber();
        rageText.text = "" + myRage;
        rageBar.size = (float)myRage / 100;
        if (rageBar.size == 1)
        {
            rageMaterial.color = new Color(1, 0, 0, 1);
            fullRageImage.enabled = true;
        }
        else
        {
            rageMaterial.color = new Color(0, 0, 1, 1);
            fullRageImage.enabled = false;
        }            
    }

    void PauseGame()
    {
        pauseCanvas.enabled = true;
        // Screen.lockCursor = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    void GameOver()
    {
        gameOverCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pauseCanvas.enabled = false;
        // Screen.lockCursor = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
