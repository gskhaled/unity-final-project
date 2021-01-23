using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionGun : MonoBehaviour
{
    public List<GameObject> currHitObjects = new List<GameObject>();

    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;
    public ParticleSystem muzzleFlash;
    public AudioSource shootingSound;

    public int killedSoFar = 0;

    private Vector3 origin;
    private Vector3 direction;
    private float currentHitDistance;
    private bool shooting = false;
    private float lastPlayed_shoot = 0f;
    private int maxClips = 3;
    private int clips = 1;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            shooting = !shooting;
        }

        if (shooting)
        {
            origin = transform.position;
            direction = transform.forward;
            currentHitDistance = maxDistance;
            currHitObjects.Clear();
            RaycastHit[] hits = Physics.SphereCastAll(origin, sphereRadius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.GetComponent<NormalLogic>() != null ||
                   hit.transform.gameObject.GetComponent<HunterLogic>() != null ||
                   hit.transform.gameObject.GetComponent<ChargerLogic>() != null ||
                   hit.transform.gameObject.GetComponent<TankLogic>() != null 
/*                   || hit.transform.gameObject.name == "Cube"*/
                   )
                {
                    currHitObjects.Add(hit.transform.gameObject);
                }

                foreach (GameObject infected in currHitObjects)
                {
                    if (infected.GetComponent<HunterLogic>() != null)
                    {
                        //infected.GetComponent<HunterLogic>().TakeDamage();
                        
                        Shoot();
                        break;
                    }
/*                    if (infected.name == "Cube")
                    {
                        Shoot();
                        break;
                    }*/

                }

                currentHitDistance = hit.distance;
            }
        }

        if(killedSoFar == 10)
        {
            if(clips<maxClips)
            clips += 1;

            killedSoFar = 0;

        }

        //cheat
        if (Input.GetKeyDown(KeyCode.F11))
        {
            clips += 1;
        }
        
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    void Shoot()
    {

        if (Time.time - lastPlayed_shoot >= 1f)
        {
            shootingSound.Play();
            muzzleFlash.Play();
            lastPlayed_shoot = Time.time;
        }

    }
}
