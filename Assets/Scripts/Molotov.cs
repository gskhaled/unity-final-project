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
                Target infected = obj.GetComponent<Target>();
                if (infected != null)
                {
                    infected.TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(delay);
            duration--;
        }
        explosionEffect.Stop();
        Destroy(gameObject);
    }
}
