using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit);
            float toGround = hit.point.y;
            transform.Translate(new Vector3(-0.2f, 0, 0));
            transform.position = new Vector3(transform.position.x, toGround + transform.localScale.y / 2, transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit);
            float toGround = hit.point.y;
            transform.Translate(new Vector3(0.2f, 0, 0));
            transform.position = new Vector3(transform.position.x, toGround + transform.localScale.y / 2, transform.position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -10, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 10, 0));
        }
    }
}
