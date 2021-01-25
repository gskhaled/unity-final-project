using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 1f;
    public AudioSource explosionSound;

    private int duration = 4;
    private int damage = 100;
    private float delay = 1f;
    private bool exploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!exploded && collision.gameObject.layer == 9)
        {
            exploded = true;
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        // Distract nearby infected
        Collider[] nearBy = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider obj in nearBy)
        {
            string tag = obj.tag;
           // Debug.Log(tag);
            switch (tag)
            {
                case "Normal":
                    {
                        NormalLogic target = obj.GetComponent<NormalLogic>();
                        if (target != null)
                        {
                            target.Distract(transform);
                        }
                        break;
                    }
                case "Charger":
                    {
                        ChargerLogic target = obj.GetComponent<ChargerLogic>();
                        if (target != null)
                        {
                            target.Distract(transform);
                        }
                        break;
                    }
                case "Tank":
                    {
                        TankLogic target = obj.GetComponent<TankLogic>();
                        if (target != null)
                        {
                            target.Distract(transform);
                        }
                        break;
                    }
                case "Hunter":
                    {
                        HunterLogic target = obj.GetComponent<HunterLogic>();
                        if (target != null)
                        {
                            target.Distract(transform);
                        }
                        break;
                    }
                case "Spitter":
                    {
                        SpitterLogic target = obj.GetComponent<SpitterLogic>();
                        if (target != null)
                        {
                            target.Distract(transform);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        // Play explosion sounds for duration time
        while (duration > 0)
        {
            explosionSound.Play();
            yield return new WaitForSeconds(delay);
            duration--;
        }

        // Give them damage
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
                case "Spitter":
                    {
                        SpitterLogic target = obj.GetComponent<SpitterLogic>();
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
        explosionEffect.SetActive(true);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
