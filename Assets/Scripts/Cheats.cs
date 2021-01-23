using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public GameObject normalInfected;
    public GameObject specialInfected1;
    public GameObject specialInfected2;
    public GameObject specialInfected3;
    public GameObject specialInfected4;
    public GameObject player;
    public GameObject FPSCam;
    public GameObject healthPack;
    public GameObject ammoPack;
    public int spawningRange = 10;
    public int hordeCount = 20;
    public int currentSceneIndex;

    private WeaponSwitching weaponHolder;
    private CollectingItems inventoryScript;

    private void Start()
    {
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();
        inventoryScript = player.GetComponentInChildren<CollectingItems>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(normalInfected, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(specialInfected1, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(specialInfected2, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(specialInfected3, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(specialInfected4, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                float maxDeviation = 5f;
                for(int i = 0; i < hordeCount; i++)
                {
                    Vector3 deviation3D = Random.insideUnitCircle * maxDeviation;
                    Quaternion rot = Quaternion.LookRotation(Vector3.forward * spawningRange + deviation3D);
                    Vector3 forwardVector = FPSCam.transform.rotation * rot * Vector3.forward;
                    RaycastHit hit;
                    if (Physics.Raycast(FPSCam.transform.position, forwardVector, out hit, spawningRange))
                    {
                        Instantiate(normalInfected, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                NormalLogic[] normalInfected = FindObjectsOfType<NormalLogic>();
                ChargerLogic[] specialInfected1 = FindObjectsOfType<ChargerLogic>();
                HunterLogic[] specialInfected2 = FindObjectsOfType<HunterLogic>();
                TankLogic[] specialInfected3 = FindObjectsOfType<TankLogic>();
                SpitterLogic[] specialInfected4 = FindObjectsOfType<SpitterLogic>();
                for(int i = 0; i < normalInfected.Length; i++)
                {
                    Destroy(normalInfected[i].gameObject);
                }
                for (int i = 0; i < specialInfected1.Length; i++)
                {
                    Destroy(specialInfected1[i].gameObject);
                }
                for (int i = 0; i < specialInfected2.Length; i++)
                {
                    Destroy(specialInfected2[i].gameObject);
                }
                for (int i = 0; i < specialInfected3.Length; i++)
                {
                    Destroy(specialInfected3[i].gameObject);
                }
                for (int i = 0; i < specialInfected4.Length; i++)
                {
                    Destroy(specialInfected4[i].gameObject);
                }
            }
            if (Input.GetKeyDown(KeyCode.KeypadPeriod))
            {
                NormalLogic[] normalInfected = FindObjectsOfType<NormalLogic>();
                ChargerLogic[] specialInfected1 = FindObjectsOfType<ChargerLogic>();
                HunterLogic[] specialInfected2 = FindObjectsOfType<HunterLogic>();
                TankLogic[] specialInfected3 = FindObjectsOfType<TankLogic>();
                SpitterLogic[] specialInfected4 = FindObjectsOfType<SpitterLogic>();
                for (int i = 0; i < normalInfected.Length; i++)
                {
                    normalInfected[i].TakeDamage(10);
                }
                for (int i = 0; i < specialInfected1.Length; i++)
                {
                    specialInfected1[i].TakeDamage(10);
                }
                for (int i = 0; i < specialInfected2.Length; i++)
                {
                    specialInfected2[i].TakeDamage(10);
                }
                for (int i = 0; i < specialInfected3.Length; i++)
                {
                    specialInfected3[i].TakeDamage(10);
                }
                for (int i = 0; i < specialInfected4.Length; i++)
                {
                    specialInfected4[i].TakeDamage(10);
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                inventoryScript.AddBomb("Molotov cheat", null);
                inventoryScript.AddBomb("PipeBomb cheat", null);
                inventoryScript.AddBomb("StunGrenade cheat", null);
            }
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(ammoPack, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, spawningRange))
                {
                    Instantiate(healthPack, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                weaponHolder.getCurrentGun().AddAmmunition();
            }
        }
    }
}
