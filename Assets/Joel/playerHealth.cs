using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    int health = 300;

    // Start is called before the first frame update
    void Start()
    {
        health = 300;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incurDamage (int damage)
    {
        health -= damage;
    }
}
