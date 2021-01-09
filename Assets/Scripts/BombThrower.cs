using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject bomb;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ThrowBomb();
        }
    }

    void ThrowBomb()
    {
        GameObject toThrow = Instantiate(bomb, transform.position, transform.rotation);
        Rigidbody rb = toThrow.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
