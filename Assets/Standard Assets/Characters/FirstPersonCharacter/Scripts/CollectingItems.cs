using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingItems : MonoBehaviour
{
    public Camera FPSCam;
    public float range = 2f;
    public Dictionary<string, int> inventory;
    public Dictionary<string, int> grenades;
    public Dictionary<string, bool> weapons;


    // Start is called before the first frame update
    void Start()
    {
        inventory= new Dictionary<string, int>();
        inventory.Add("Bile", 0);
        inventory.Add("Alcohol", 0);
        inventory.Add("Canister", 0);
        inventory.Add("GunPowder", 0);
        inventory.Add("Rag", 0);
        inventory.Add("Sugar", 0);
        grenades = new Dictionary<string, int>();
        grenades.Add("Molotov", 0);
        grenades.Add("PipeBomb", 0);
        grenades.Add("StunGrenade", 0);
        grenades.Add("Health Pack", 0);
        weapons = new Dictionary<string, bool>();
        weapons.Add("AR", false);
        weapons.Add("Hunting Rifle", false);
        weapons.Add("Pistol", false);
        weapons.Add("Shotgun", false);
        weapons.Add("SMG", false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
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
                if (tag.Equals("Usable Health Pack"))
                {
                    grenades["Health Pack"] = grenades["Health Pack"] + 1;
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Health Pack: " + grenades["Health Pack"]);
                }
                else if (tag.Equals("Usable Molotov"))
                {
                    if (grenades["Molotov"] < 3)
                    {
                        grenades["Molotov"] = grenades["Molotov"] + 1;
                    }
                    Destroy(hit.transform.gameObject);
                    Debug.Log("Molotov: " + grenades["Molotov"]);
                }
                else if (tag.Equals("Usable Pipe Bomb"))
                {
                    if (grenades["PipeBomb"] < 2)
                    {
                        grenades["PipeBomb"] = grenades["PipeBomb"] + 1;
                    }
                    Destroy(hit.transform.gameObject);
                    Debug.Log("PipeBomb: " + grenades["PipeBomb"]);
                }
                else if (tag.Equals("Usable Stun Grenade"))
                {
                    if (grenades["StunGrenade"] < 2)
                    {
                        grenades["StunGrenade"] = grenades["StunGrenade"] + 1;
                    }
                    Destroy(hit.transform.gameObject);
                    Debug.Log("StunGrenade: " + grenades["StunGrenade"]);
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
            }
            else
            {
                return;
            }
        }

    }
}
