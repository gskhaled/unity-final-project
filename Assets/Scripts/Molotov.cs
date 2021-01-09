using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public float explosionRadius = 1f;

    private int duration = 5;
    private int damage = 25;
    private float delay = 1f;

    private bool hasExploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        hasExploded = true;
        explosionEffect.Play();
        Collider[] nearBy = Physics.OverlapSphere(transform.position, explosionRadius);
        while(duration > 0)
        {
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
