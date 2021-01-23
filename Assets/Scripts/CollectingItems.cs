using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingItems : MonoBehaviour
{
    public Camera FPSCam;
    public float range = 2f;
    public GameObject weapon;
    public AudioSource ammoPickupSound;
    public bool startWithAPistol = true;
    private Dictionary<string, int> inventory = new Dictionary<string, int>();
    private Dictionary<string, int> bombs = new Dictionary<string, int>();
    private Dictionary<string, bool> weapons = new Dictionary<string, bool>();
    // Start is called before the first frame update
    void Awake()
    {
        inventory.Add("Bile", 0);
        inventory.Add("Alcohol", 0);
        inventory.Add("Canister", 0);
        inventory.Add("GunPowder", 0);
        inventory.Add("Rag", 0);
        inventory.Add("Sugar", 0);
        
        bombs.Add("Molotov", 0);
        bombs.Add("PipeBomb", 0);
        bombs.Add("StunGrenade", 0);
        //bombs.Add("Health Pack", 0);
        
        weapons.Add("AR", false);
        weapons.Add("Hunting Rifle", false);
        weapons.Add("Pistol", startWithAPistol);
        weapons.Add("Shotgun", false);
        weapons.Add("SMG", false);
    }
    public Dictionary<string, int> getInventory()
    {
        return inventory;
    }

    public Dictionary<string, int> getBombs()
    {
        return bombs;
    }

    public Dictionary<string, bool> getWeapons()
    {
        return weapons;
    }

    public void useBombs(string bombName)
    {

        if (bombs.ContainsKey(bombName))
        {
            bombs[bombName] = --bombs[bombName];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range))
        {
            string tag = hit.transform.tag;
            if (tag == null)
                return;
            if (tag.Contains("Collectable"))
            {
                if (tag.Equals("Collectable Bile"))
                {
                    inventory["Bile"] = inventory["Bile"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Bile: " + inventory["Bile"]);
                }
                else if (tag.Equals("Collectable Alcohol"))
                {
                    inventory["Alcohol"] = inventory["Alcohol"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Alcohol: " + inventory["Alcohol"]);
                }
                else if (tag.Equals("Collectable Canister"))
                {
                    inventory["Canister"] = inventory["Canister"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Canister: " + inventory["Canister"]);
                }
                else if (tag.Equals("Collectable Gun Powder"))
                {
                    inventory["GunPowder"] = inventory["GunPowder"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("GunPowder: " + inventory["GunPowder"]);
                }
                else if (tag.Equals("Collectable Rag"))
                {
                    inventory["Rag"] = inventory["Rag"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Rag: " + inventory["Rag"]);
                }
                else if (tag.Equals("Collectable Sugar"))
                {
                    inventory["Sugar"] = inventory["Sugar"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Sugar: " + inventory["Sugar"]);
                }
            }
            else if (tag.Contains("Usable"))
            {
               
                if (tag.Equals("Usable Molotov"))
                {
                    AddBomb("Molotov", hit.transform);
                    Debug.Log("Molotov: " + bombs["Molotov"]);
                }
                else if (tag.Equals("Usable Pipe Bomb"))
                {
                    AddBomb("PipeBomb", hit.transform);
                    Debug.Log("PipeBomb: " + bombs["PipeBomb"]);
                }
                else if (tag.Equals("Usable Stun Grenade"))
                {
                    AddBomb("StunGrenade", hit.transform);
                    Debug.Log("StunGrenade: " + bombs["StunGrenade"]);
                }
            }
            else if (tag.Contains("Weapon"))
            {
                if (tag.Equals("Weapon AR"))
                {
                    weapons["AR"] = true;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("AR: " + weapons["AR"]);
                }
                else if (tag.Equals("Weapon Hunting Rifle"))
                {
                    weapons["Hunting Rifle"] = true;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Hunting Rifle: " + weapons["Hunting Rifle"]);
                }
                else if (tag.Equals("Weapon Pistol"))
                {
                    weapons["Pistol"] = true;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Pistol: " + weapons["Pistol"]);
                }
                else if (tag.Equals("Weapon SMG"))
                {
                    weapons["SMG"] = true;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("SMG: " + weapons["SMG"]);
                }
                else if (tag.Equals("Weapon Shotgun"))
                {
                    weapons["Shotgun"] = true;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Shotgun: " + weapons["Shotgun"]);
                }
            }else if (tag.Contains("Consumable"))
            {
                if (tag.Equals("Consumable Helath Pack"))
                {
                    AddBomb("Health Pack", hit.transform);
                }
                else if (tag.Equals("Consumable Ammo"))
                {
                    if(weapon.GetComponent<WeaponSwitching>() != null)
                        weapon.GetComponent<WeaponSwitching>().AddAmmmunition();
                    Destroy(hit.transform.gameObject);
                    ammoPickupSound.Play();
                    Debug.Log("Consumable Ammo: Filled");
                }

            }
            else
            {
                return;
            }
        }

    }

    public void AddBomb(string name, Transform t)
    {
        bool cheating = false;
        if (name.Contains("cheat"))
        {
            name = name.Split()[0];
            cheating = true;
        }
        try
        {
            if (name.Equals("Molotov") && bombs["Molotov"] < 3)
            {
                bombs["Molotov"] = bombs["Molotov"] + 1;
                Debug.Log("Added Molotov: " + bombs["Molotov"]);
                if (t != null)
                    Destroy(t.gameObject);
                else
                    if (!cheating)
                        Recipe(name);
            }
            else if (name.Equals("PipeBomb") && bombs["PipeBomb"] < 2)
            {
                bombs["PipeBomb"] = bombs["PipeBomb"] + 1;
                Debug.Log("Added PipeBomb: " + bombs["PipeBomb"]);
                if (t != null)
                    Destroy(t.gameObject);
                else
                    if (!cheating)
                        Recipe(name);
            }
            else if (name.Equals("StunGrenade") && bombs["StunGrenade"] < 2)
            {
                bombs["StunGrenade"] = bombs["StunGrenade"] + 1;
                Debug.Log("Added StunGrenade: " + bombs["StunGrenade"]);
                if (t != null)
                    Destroy(t.gameObject);
                else
                    if (!cheating)
                        Recipe(name);
            }
            else if(name.Equals("Health Pack"))
            {
                // Call method for Consuming Health Pack
                gameObject.GetComponent<playerHealth>().increaseHealth(50);
                Debug.Log("Added and consumed Health Pack");
                if (t != null)
                    Destroy(t.gameObject);
                else
                    if (!cheating)
                        Recipe(name);
            }
            else
            {
                Debug.Log("Item " + name + " cannot be abdded to the inventory.");
            }
        }
        catch
        {
            Debug.Log("Error, item " + name + " cannot be abdded to the inventory.");
        }
    }

    void Recipe(string name)
    {
        try
        {
            switch (name)
            {
                case ("Molotov"):
                    inventory["Alcohol"] = inventory["Alcohol"] - 2;
                    inventory["Rag"] = inventory["Rag"] - 2;
                    break;
                case ("StunGrenade"):
                    inventory["GunPowder"] = inventory["GunPowder"] - 2;
                    inventory["Sugar"] = inventory["Sugar"] - 1;
                    break;
                case ("PipeBomb"):
                    inventory["Alcohol"] = inventory["Alcohol"] - 2;
                    inventory["GunPowder"] = inventory["GunPowder"] - 1;
                    inventory["Canister"] = inventory["Canister"] - 1;
                    break;
                case ("Health Pack"):
                    inventory["Alcohol"] = inventory["Alcohol"] - 2;
                    inventory["Rag"] = inventory["Rag"] - 2;
                    break;
                default:
                    Debug.Log("Item " + name + " is not available in recipe.");
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Got error while checking for recipe" + e.Message);
        }
    }
}
