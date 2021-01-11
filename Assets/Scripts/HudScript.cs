using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{
    public Scrollbar healthBar;
    public Material healthBarMaterial;
    public Text healthText;

    private int myHealth = 100;

    void Start()
    { 
        healthBar.size = 1.0f;
        healthBarMaterial.color = new Color(1, 0, 0, 1);
    }

    
    void Update()
    {
        health();
    }

    void health()
    {
        healthText.text = "" + myHealth;
        healthBar.size = (float)myHealth / 300;
        if (healthBar.size <= 0.35)
        {
            healthBarMaterial.color = new Color(1, 0, 0, 1);
        }
        else
            healthBarMaterial.color = new Color(0, 1, 0, 1);
    }
}
