using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGrenade : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public float explosionRadius = 1f;

    private int duration = 3;

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        explosionEffect.Play();
        Collider[] nearBy = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider obj in nearBy)
        {
            Target infected = obj.GetComponent<Target>();
            if (infected != null)
            {
                // ATTRACT INFECTED!!!!!
            }
        }
        yield return new WaitForSeconds(duration);
        foreach (Collider obj in nearBy)
        {
            Target infected = obj.GetComponent<Target>();
            if (infected != null)
            {
                // UN ATTRACT INFECTED!!!!!
            }
        }
        Destroy(gameObject);
    }
}
