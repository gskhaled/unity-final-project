using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationScript : MonoBehaviour
{
    public float radius = 5.0f;
    public GameObject normal;
    public GameObject special;
    public int infectedSize = 5;
    Vector3 originPoint;
    public int specialSize = 2;
    public Transform player;
    private float distance;
    public float distanceDifference = 15.0f;
    // Start is called before the first frame update
    /*  void Start()
      {





      }*/
    public void AgentSpawner()
    {
        for (int i = 0; i < infectedSize; i++)
            CreateAgent(normal);

        for (int i = 0; i < specialSize; i++)
            CreateAgent(special);
    }
    public void CreateAgent(GameObject spawned)
    {
        originPoint = this.gameObject.transform.position;
        originPoint.x += Random.Range(-radius, radius);
        originPoint.z += Random.Range(-radius, radius);
        float directionFacing = Random.Range(0f, 360f);


        Instantiate(spawned, originPoint, Quaternion.Euler(new Vector3(0f, directionFacing, 0f)));
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < distanceDifference) {
            AgentSpawner();
            Destroy(this.gameObject);
        }
         

    }






}
