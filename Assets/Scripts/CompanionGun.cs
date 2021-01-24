using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class CompanionGun : MonoBehaviour
{
    public Camera camera;
    public ThirdPersonCharacter gunHolder;
    public List<GameObject> currHitObjects = new List<GameObject>();
    public int damage = 36;
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
    private int gunAmmo = 0;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            shooting = !shooting;
            gunHolder.GetComponent<AICharacterControl>().SetTarget(camera.transform);

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
                   || hit.transform.gameObject.name == "Cube"
                   )
                {
                    currHitObjects.Add(hit.transform.gameObject);
                }

                foreach (GameObject infected in currHitObjects)
                {
                    if (infected.GetComponent<HunterLogic>() != null)
                    {
                        
                        gunHolder.GetComponent<AICharacterControl>().SetTarget(infected.transform);
                        Shoot(infected);
                        break;
                    }
                    else if (infected.GetComponent<ChargerLogic>() != null)
                    {

                        gunHolder.GetComponent<AICharacterControl>().SetTarget(infected.transform);
                        Shoot(infected);
                        break;
                    }
                    else if (infected.GetComponent<TankLogic>() != null)
                    {

                        gunHolder.GetComponent<AICharacterControl>().SetTarget(infected.transform);
                        Shoot(infected);
                        break;
                    }
                    else if (infected.GetComponent<NormalLogic>() != null)
                    {

                        gunHolder.GetComponent<AICharacterControl>().SetTarget(infected.transform);
                        Shoot(infected);
                        break;
                    }
                    else if (infected.name == "Cube")
                    {

                        gunHolder.GetComponent<AICharacterControl>().SetTarget(infected.transform);
                        //gunHolder.GetComponent<Animation>().Play("Shooting");
                        Shoot(); 
                        //Debug.Log(count);
                        break;
                    }

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
        if (Input.GetKeyDown(KeyCode.M))
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

    void Shoot(GameObject inf)
    {

        if (Time.time - lastPlayed_shoot >= 1f && gunAmmo< 5*clips)
        {
            gunAmmo += 1;
            shootingSound.Play();
            muzzleFlash.Play();
            inf.GetComponent<HunterLogic>().TakeDamage(damage);
            lastPlayed_shoot = Time.time;
        }

    }
    void Shoot()
    {

        if (Time.time - lastPlayed_shoot >= 1f && gunAmmo < 15 * clips)
        {
            gunAmmo += 1;
            shootingSound.Play();
            muzzleFlash.Play();
            
            lastPlayed_shoot = Time.time;
        }

    }
    public void getAmmoCount()
    {
        return (clips * 15) - gunAmmo;
    }
}
