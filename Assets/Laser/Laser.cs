using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Laser : MonoBehaviour
{
    NavMeshAgent agent;
    LineRenderer lineRenderer;
    public Transform laserHit;
    Vector3 fixedHit;
    bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.gameObject.CompareTag("Charger"))
        {
            if (agent.hasPath && laserHit != null)
            {
                if (first)
                {
                    fixedHit = laserHit.position;
                    first = false;
                }
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, fixedHit);
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
                first = true;
            }

        }
        else
        {
            if (agent.hasPath && laserHit != null)
            {
               
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, laserHit.position);
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
              
            }
        }


    }
}
