using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public float explosionRadius = 1f;
    public AudioSource explosionSound;

    private int duration = 4;
    private int damage = 25;
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
        /*foreach (Collider obj in nearBy)
        {
            Infected infected = obj.GetComponent<Infected>();
            if (infected != null)
            {
                infected.Distract(transform.position);
            }
        }*/

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
            Target infected = obj.GetComponent<Target>();
            if (infected != null)
            {
                infected.TakeDamage(damage);
            }
        }
        explosionEffect.SetActive(true);
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
