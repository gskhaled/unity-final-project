using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public CollectingItems CollectingItemsScript;
    
    private List<int> availableWeapons = new List<int>(); // --> array of weapon indices
    private int selectedWeapon = 0;
    private bool weaponWasPickedUp = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        // Get the available weapons in the inventory
        Dictionary<string, bool> weapons = CollectingItemsScript.getWeapons();
        foreach (var item in weapons)
        {
            // If the weapon is picked up, add its index (as a child)
            // to the list of available weapon indices.
            if (item.Value)
            {
                weaponWasPickedUp = true;
                availableWeapons.Add(getWeaponIndex(item.Key));
            }
        }

        if (weaponWasPickedUp)
        {
            // Deactivate the current weapon
            transform.GetChild(availableWeapons[selectedWeapon]).gameObject.SetActive(false);
            // Change weapons in case you have more than 1
            if (availableWeapons.Count == 1 || selectedWeapon > availableWeapons.Count - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon++;
            }
            // Activate the newly selected weapon
            transform.GetChild(availableWeapons[selectedWeapon]).gameObject.SetActive(true);
        }
    }

    int getWeaponIndex(string name)
    {
        bool found = false;
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (name == weapon.gameObject.name)
            {
                found = true;
                break;
            }
            i++;
        }
        if (found)
            return i;
        else
            return -1;
    }
}
