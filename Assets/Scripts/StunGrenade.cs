using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGrenade : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public float explosionRadius = 1f;
    public AudioSource explosionSound;

    private int duration = 3;
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
        explosionEffect.Play();
        explosionSound.Play();

        // Stun all nearby infected
        Collider[] nearBy = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider obj in nearBy)
        {
            string tag = obj.tag;
            Debug.Log(tag);
            switch (tag)
            {
                case "Normal":
                    {
                        NormalLogic target = obj.GetComponent<NormalLogic>();
                        if (target != null)
                        {
                            target.Stun();
                        }
                        break;
                    }
                case "Charger":
                    {
                        ChargerLogic target = obj.GetComponent<ChargerLogic>();
                        if (target != null)
                        {
                            target.Stun();
                        }
                        break;
                    }
                case "Tank":
                    {
                        TankLogic target = obj.GetComponent<TankLogic>();
                        if (target != null)
                        {
                            target.Stun();
                        }
                        break;
                    }
                case "Hunter":
                    {
                        HunterLogic target = obj.GetComponent<HunterLogic>();
                        if (target != null)
                        {
                            target.Stun();
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        // Destroy after duration time
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
