using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 1f;

    private int duration = 4;
    private int damage = 25;
    private float delay = 1f;
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        Collider[] nearBy = Physics.OverlapSphere(transform.position, explosionRadius);
        while(duration > 0)
        {
            foreach (Collider obj in nearBy)
            {
                Target infected = obj.GetComponent<Target>();
                if (infected != null)
                {
                    // ATTRACT INFECTED!!!!!
                }
            }
            yield return new WaitForSeconds(delay);
            duration--;
        }
        foreach (Collider obj in nearBy)
        {
            Target infected = obj.GetComponent<Target>();
            if (infected != null)
            {
                infected.TakeDamage(damage);
            }
        }
        explosionEffect.SetActive(true);
        Destroy(gameObject);
    }
}
