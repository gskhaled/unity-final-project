using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public float explosionRadius = 1f;
    public AudioSource explosionSound;

    private int duration = 5;
    private int damage = 25;
    private float delay = 1f;
    private bool exploded = false;
    private void Start()
    {
        explosionEffect.Stop();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!exploded && collision.gameObject.layer == 9)
        {
            exploded = true;
            StartCoroutine(Explode());
        }
  
    }

    IEnumerator Explode()
    {
        explosionEffect.Play();
        explosionSound.Play();
        while(duration > 0)
        {
            // Give nearby damage over time.
            Collider[] nearBy = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider obj in nearBy)
            {
                string tag = obj.tag;
                switch (tag)
                {
                    case "Target":
                        {
                            Target target = obj.GetComponent<Target>();
                            if (target != null)
                            {
                                target.TakeDamage((int)damage);
                            }
                            break;
                        }
                    case "Normal":
                        {
                            NormalLogic target = obj.GetComponent<NormalLogic>();
                            if (target != null)
                            {
                                target.TakeDamage((int)damage);
                            }
                            break;
                        }
                    case "Charger":
                        {
                            ChargerLogic target = obj.GetComponent<ChargerLogic>();
                            if (target != null)
                            {
                                target.TakeDamage((int)damage);
                            }
                            break;
                        }
                    case "Tank":
                        {
                            TankLogic target = obj.GetComponent<TankLogic>();
                            if (target != null)
                            {
                                target.TakeDamage((int)damage);
                            }
                            break;
                        }
                    case "Hunter":
                        {
                            HunterLogic target = obj.GetComponent<HunterLogic>();
                            if (target != null)
                            {
                                target.TakeDamage((int)damage);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            yield return new WaitForSeconds(delay);
            duration--;
        }
        explosionEffect.Stop();
        Destroy(gameObject);
    }
}
