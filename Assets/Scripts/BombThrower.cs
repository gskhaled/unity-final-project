using System.Collections.Generic;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    [System.Serializable]
    public class Bomb
    {
        public enum BombType
        {
            Molotov,
            StunGrenade,
            PipeBomb
        }
        public BombType type;
        public GameObject bomb;
    }
    public Camera FPSCam;
    public Bomb[] bombs;
    public float throwForce = 40f;
    public CollectingItems CollectingItemsScript;

    private Dictionary<string, int> availableBombs;
    private List<string> bombNames = new List<string>();
    private int currentBomb = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && bombNames.Count != 0)
        {
            ThrowBomb();
        }

        if (Input.GetKeyDown(KeyCode.Z) && doIHaveBombs())
        {
            if (bombNames.Count == 1 || currentBomb >= bombNames.Count - 1)
                currentBomb = 0;
            else
                currentBomb++;

            Debug.Log("CURRENTLY HOLDING: " + bombNames[currentBomb]);
        }
    }

    bool doIHaveBombs()
    {
        availableBombs = CollectingItemsScript.getBombs();

        bombNames.Clear();
        bool foundBombs = false;
        foreach(var item in availableBombs)
        {
            if(item.Value != 0)
            {
                bombNames.Add(item.Key);
                foundBombs = true;
            }
        }
        return foundBombs;
    }

    void ThrowBomb()
    {
        int bombIndex = -1;
        for (int i = 0; i < bombs.Length; i++)
        {
            if (bombNames[currentBomb] == bombs[i].type.ToString())
            {
                bombIndex = i;
                break;
            }
        }
        if (bombIndex < 0)
            return;

        GameObject toThrow = Instantiate(bombs[bombIndex].bomb, FPSCam.transform.position, FPSCam.transform.rotation);
        Rigidbody rb = toThrow.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        rb.useGravity = true;

        CollectingItemsScript.useBombs(bombNames[currentBomb]);
        doIHaveBombs();
    }
}
