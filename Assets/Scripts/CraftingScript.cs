using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftingScript : MonoBehaviour
{
    public CollectingItems CollectingItemsScript;
    public Text alcoholText;
    public Text ragsText;
    public Text sugarText;
    public Text GPText;
    public Text canText;
    public Text molotovAmountText;
    public Text pipeAmountText;
    public Text stunAmountText;
    public Canvas craftingCanvas;

    public Camera UIcamera;

    public Button molotovCraft;
    public Button stunCraft;
    public Button pipeCraft;
    public Button healthCraft;

    public GameObject[] rotate;
    public GameObject[] rotate2;

    private Dictionary<string, int> MolotovCrafting = new Dictionary<string, int>();
    private Dictionary<string, int> StunCrafting = new Dictionary<string, int>();
    private Dictionary<string, int> PipeCrafting = new Dictionary<string, int>();
    private Dictionary<string, int> HealthCrafting = new Dictionary<string, int>();

    void Start()
    {
        MolotovCrafting.Add("Alcohol", 2);
        MolotovCrafting.Add("Rag", 2);

        StunCrafting.Add("GunPowder", 2);
        StunCrafting.Add("Sugar", 1);

        PipeCrafting.Add("Alcohol", 2);
        PipeCrafting.Add("GunPowder", 1);
        PipeCrafting.Add("Canister", 1);

        HealthCrafting.Add("Alcohol", 2);
        HealthCrafting.Add("Rag", 2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            openCrafting();

        if (craftingCanvas.isActiveAndEnabled)
        {
            Dictionary<string, int> inventory = CollectingItemsScript.getInventory();
            Dictionary<string, int> grenades = CollectingItemsScript.getBombs();

            Collectables(inventory);

            foreach (var item in grenades)
            {
                switch (item.Key)
                {
                    case "Molotov":
                        molotovAmountText.text = item.Value + "/3";
                        break;
                    case "StunGrenade":
                        stunAmountText.text = item.Value + "/2";
                        break;
                    case "PipeBomb":
                        pipeAmountText.text = item.Value + "/2";
                        break;
                }
            }

            molotovCraft.interactable = CheckForCrafting("MolotovCocktail", inventory, grenades);
            stunCraft.interactable = CheckForCrafting("StunGrenade", inventory, grenades);
            pipeCraft.interactable = CheckForCrafting("PipeBomb", inventory, grenades);
            healthCraft.interactable = CheckForCrafting("HealthPack", inventory, grenades);
        }
        foreach(var item in rotate)
        {
            item.transform.Rotate(new Vector3(0, 0, 8), Space.Self);
        }
        foreach (var item in rotate2)
        {
            item.transform.Rotate(new Vector3(0, 8, 0), Space.Self);
        }
    }

    void FixedUpdate()
    {
    }

    void Collectables(Dictionary<string, int> inventory)
    {
        foreach (var item in inventory)
        {
            switch (item.Key)
            {
                case "Alcohol":
                    alcoholText.text = "x" + item.Value;
                    break;
                case "Rag":
                    ragsText.text = "x" + item.Value;
                    break;
                case "Canister":
                    canText.text = "x" + item.Value;
                    break;
                case "Sugar":
                    sugarText.text = "x" + item.Value;
                    break;
                case "GunPowder":
                    GPText.text = "x" + item.Value;                   
                    break;
            }
        }
    }

    bool CheckForCrafting(string toBeCrafted, Dictionary<string, int> inventory, Dictionary<string, int> grenades)
    {
        switch (toBeCrafted)
        {
            case "MolotovCocktail":
                foreach(var item in MolotovCrafting)
                {
                    foreach(var item2 in inventory)
                    {
                        if (item.Key == item2.Key)
                        {
                            if (item.Value > item2.Value)
                                return false;
                        }
                    }
                }

                foreach (var item in grenades)
                {
                    if (item.Key == "Molotov")
                    {
                        if (item.Value >= 3)
                            return false;
                    }
                }
                break;

            case "StunGrenade":
                foreach (var item in StunCrafting)
                {
                    foreach (var item2 in inventory)
                    {
                        if (item.Key == item2.Key)
                        {
                            if (item.Value > item2.Value)
                                return false;
                        }
                    }
                }
                
                foreach (var item in grenades)
                {
                    if (item.Key == "StunGrenade")
                    {
                        if (item.Value >= 2)
                            return false;
                    }
                }
                break;

            case "PipeBomb":
                foreach (var item in PipeCrafting)
                {
                    foreach (var item2 in inventory)
                    {
                        if (item.Key == item2.Key)
                        {
                            if (item.Value > item2.Value)
                                return false;
                        }
                    }
                }
                
                foreach (var item in grenades)
                {
                    if (item.Key == "PipeBomb")
                    {
                        if (item.Value >= 2)
                            return false;
                    }
                }
                break;

            case "HealthPack":
                foreach (var item in HealthCrafting)
                {
                    foreach (var item2 in inventory)
                    {
                        if (item.Key == item2.Key)
                        {
                            if (item.Value > item2.Value)
                                return false;
                        }
                    }
                }
                break;
        }
        return true;
    }

    public void addGrenades(string Name)
    {
        switch (Name)
        {
            case "Molotov":
                //CollectingItemsScript.addBombs(Molotov);
                break;
            case "StunGrenade":
                //CollectingItemsScript.addBombs(StunGrenade);
                break;
            case "PipeBomb":
                //CollectingItemsScript.addBombs(PipeBomb);
                break;
            case "HealthPack":
                //myHealth += 50;
        }
    }

    void openCrafting()
    {
        UIcamera.enabled = true;
        craftingCanvas.enabled = true;
        Screen.lockCursor = false;
        Time.timeScale = 0;
    }

    public void exitCreafting()
    {
        UIcamera.enabled = false;
        craftingCanvas.enabled = false;
        Screen.lockCursor = true;
        Time.timeScale = 1;
    }
}
