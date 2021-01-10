using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 0f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxMagazine = 10;
    public float allAmmu = Mathf.Infinity;
    public float reloadTime = 2f;
    public Camera FPSCam;
    public ParticleSystem muzzleFlash;
    public Animator animator;
    public GameObject impactEffect;
    public float impactDuration = 0.2f;
    public float impactForce = 1f;
    public AudioSource shootingSound;
    public AudioSource reloadingSound;

    private float nextTimeToFire = 0f;
    private int currentAmmo = 0;
    private bool isReloading = false;
    private float lastPlayed_shoot = 0f;
    void Start()
    {
        currentAmmo = maxMagazine;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    void Update()
    {
        if (isReloading)
            return;

        if(allAmmu > 0)
        {
            if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxMagazine))
            {
                StartCoroutine(Reload());
                return;
            }
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + (1f / fireRate);
                Shoot();
            }
        }
    }

    IEnumerator Reload()
    {
        if (allAmmu > 0)
        {
            isReloading = true;
            reloadingSound.Play();
            //Debug.Log("Reloading.....");
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadTime - .25f);
            animator.SetBool("Reloading", false);
            yield return new WaitForSeconds(.25f);
            int ammu = (int) allAmmu - maxMagazine;
            if (ammu >= 0)
            {
                currentAmmo = maxMagazine;
            }
            else
                currentAmmo = (int) allAmmu;
            allAmmu = ammu;
            isReloading = false;
        }
    }

    void Shoot()
    {
        if(Time.time - lastPlayed_shoot >= .1f)
        {
            shootingSound.Play();
            lastPlayed_shoot = Time.time;
        }
        muzzleFlash.Play();
        currentAmmo--;
        RaycastHit hit;
        if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
        GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactObj, impactDuration);
    }
}
