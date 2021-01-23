using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool isShotgun = false;
    public float damage = 0f;
    public float range = 100f;
    public float fireRate = 1f;
    public int maxMagazine = 10;
    public float allAmmo = Mathf.Infinity;
    public float MAXAmmo = Mathf.Infinity;
    public float reloadTime = 2f;
    public Camera FPSCam;
    public ParticleSystem muzzleFlash;
    public Animator animator;
    public GameObject impactEffect;
    public float impactDuration = 0.2f;
    public float impactForce = 1f;
    public AudioSource shootingSound;
    public AudioSource reloadingSound;
    public int currentAmmo = 0;

    private float nextTimeToFire = 0f;
    private bool isReloading = false;
    private bool shooting = false;
    private playerHealth player;

    private float lastPlayed_shoot = 0f;
    void Start()
    {
        currentAmmo = maxMagazine;
        player = FindObjectOfType<playerHealth>();
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (isReloading)
            return;

        if(currentAmmo > 0 || (currentAmmo <= 0 && allAmmo > 0))
        {
            if (allAmmo > 0 && (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && currentAmmo != maxMagazine)))
            {
                StartCoroutine(Reload());
                return;
            }
            if (!isShotgun) // Shotgun is semi automatic!
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    shooting = true;

                    if (Time.time >= nextTimeToFire)
                    {
                        nextTimeToFire = Time.time + (60f / fireRate);
                        if (player.isRaging())
                            Shoot(damage * 2);
                        else
                            Shoot(damage);
                    }
                }
                else
                {
                    shooting = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    shooting = true;

                    if (Time.time >= nextTimeToFire)
                    {
                        nextTimeToFire = Time.time + (60f / fireRate);
                        if (player.isRaging())
                            Shoot(damage * 2);
                        else
                            Shoot(damage);
                    }
                }
                else
                {
                    shooting = false;
                }
            }
        }
    }

    IEnumerator Reload()
    {
        if (allAmmo > 0)
        {
            isReloading = true;
            reloadingSound.Play();
            //Debug.Log("Reloading.....");
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadTime - .25f);
            animator.SetBool("Reloading", false);
            yield return new WaitForSeconds(.25f);

            int ammo = (int) allAmmo - maxMagazine + currentAmmo;
            if (ammo >= 0)
            {
                currentAmmo = maxMagazine;
            }
            else
                currentAmmo = (int) allAmmo;
            allAmmo = ammo;
            isReloading = false;
        }
    }

    void Shoot(float damage)
    {
        if(Time.time - lastPlayed_shoot >= .1f)
        {
            shootingSound.Play();
            lastPlayed_shoot = Time.time;
        }
        muzzleFlash.Play();
        currentAmmo--;
        if (!isShotgun)
        {
            RaycastHit hit;
            if (Physics.Raycast(FPSCam.transform.position, FPSCam.transform.forward, out hit, range))
            {
                GameObject obj = hit.transform.gameObject;
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
                            if (target.GetComponent<Rigidbody>() != null)
                            {
                                hit.rigidbody.AddForce(-hit.normal * impactForce);
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
                            if (target.GetComponent<Rigidbody>() != null)
                            {
                                hit.rigidbody.AddForce(-hit.normal * impactForce);
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
                            if (target.GetComponent<Rigidbody>() != null)
                            {
                                hit.rigidbody.AddForce(-hit.normal * impactForce);
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
                            if (target.GetComponent<Rigidbody>() != null)
                            {
                                hit.rigidbody.AddForce(-hit.normal * impactForce);
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
                            if (target.GetComponent<Rigidbody>() != null)
                            {
                                hit.rigidbody.AddForce(-hit.normal * impactForce);
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObj, impactDuration);
        }
        else
        {
            int pellets = 10;
            float maxDeviation = 5f;

            for (int i = 0; i < pellets; i++)
            {
                Vector3 deviation3D = Random.insideUnitCircle * maxDeviation;
                Quaternion rot = Quaternion.LookRotation(Vector3.forward * range + deviation3D);
                Vector3 forwardVector = FPSCam.transform.rotation * rot * Vector3.forward;

                RaycastHit hit;
                if (Physics.Raycast(FPSCam.transform.position, forwardVector, out hit, range))
                {
                    GameObject obj = hit.transform.gameObject;
                    string tag = obj.tag;
                    switch (tag)
                    {
                        case "Target":
                            {
                                Target target = obj.GetComponent<Target>();
                                if (target != null)
                                {
                                    target.TakeDamage((int)damage / pellets);
                                }
                                if (target.GetComponent<Rigidbody>() != null)
                                {
                                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                                }
                                break;
                            }
                        case "Normal":
                            {
                                NormalLogic target = obj.GetComponent<NormalLogic>();
                                if (target != null)
                                {
                                    target.TakeDamage((int)damage / pellets);
                                }
                                if (target.GetComponent<Rigidbody>() != null)
                                {
                                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                                }
                                break;
                            }
                        case "Charger":
                            {
                                ChargerLogic target = obj.GetComponent<ChargerLogic>();
                                if (target != null)
                                {
                                    target.TakeDamage((int)damage / pellets);
                                }
                                if (target.GetComponent<Rigidbody>() != null)
                                {
                                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                                }
                                break;
                            }
                        case "Tank":
                            {
                                TankLogic target = obj.GetComponent<TankLogic>();
                                if (target != null)
                                {
                                    target.TakeDamage((int)damage / pellets);
                                }
                                if (target.GetComponent<Rigidbody>() != null)
                                {
                                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                                }
                                break;
                            }
                        case "Hunter":
                            {
                                HunterLogic target = obj.GetComponent<HunterLogic>();
                                if (target != null)
                                {
                                    target.TakeDamage((int)damage / pellets);
                                }
                                if (target.GetComponent<Rigidbody>() != null)
                                {
                                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                                }
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
                GameObject impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactObj, impactDuration);
            }
        }
    }

    public void AddAmmunition()
    {
        allAmmo = MAXAmmo;
    }
    public bool isShooting()
    {
        return shooting;
    }
}
