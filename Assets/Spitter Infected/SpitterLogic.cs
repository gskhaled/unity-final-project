using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpitterLogic : MonoBehaviour
{
    public GameObject bile;

    NavMeshAgent agent;
    Transform player;
    WeaponSwitching weaponHolder;
    playerHealth healthComponent;
    public Vector3 spitPosition;
    public GameObject acidBall;

    Animator animator;
    Laser laser;
    AudioSource spitClip;
    AudioSource dieClip;

    //Patroling
    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointTranslation = 5f;
    int randomDirection;

    //States
    float sightRange = 10f;
    float firingRange = 20f;
    bool playerInSightRange, playerIsFiring;
    bool isDead = false;
    bool isHit = false;
    bool isDistracted = false;
    bool isStunned = false;
    bool isSpitting = false;
    bool waiting = false;

    //Health 
    int health = 100;

    //Joel 
    GameObject playerScript;

    private void Start()
    {
        randomDirection = Random.Range(0, 2);
        agent = GetComponent<NavMeshAgent>();
        GameObject joel = GameObject.FindGameObjectWithTag("Joel");
        player = joel.transform;
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();
        healthComponent = joel.GetComponent<playerHealth>();
        animator = GetComponent<Animator>();
        laser = GetComponent<Laser>();
        spitClip = transform.GetChild(4).GetComponent<AudioSource>();
        dieClip = transform.GetChild(5).GetComponent<AudioSource>();
        // playerScript = GameObject.FindGameObjectWithTag("Joel").GetComponent<playerHealth>();
        SearchWalkPoint();
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject joel = GameObject.FindGameObjectWithTag("Joel");
        player = joel.transform;
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();
        healthComponent = joel.GetComponent<playerHealth>();

    }

    private void Update()
    {
        if (!isDead)
        {

            if (((!playerInSightRange && !isSpitting && !isDistracted) || waiting || isDistracted)  && !isHit)
            {
                if (walkPointSet) Patroling();
                else SearchWalkPoint();
            }
            
            //Check for sight, attack, and firing ranges

            if (isInLineOfSight() && isInFront() && !isDistracted && !waiting)
            {
                if (!isSpitting)
                {
                    Vector3 distVector = player.position - transform.position;
                    distVector.y = 0;
                    Quaternion angle = Quaternion.LookRotation(distVector);
                    transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * agent.speed);
                    playerInSightRange = true;
                }
            }
            else
                playerInSightRange = false;

            Gun currWeapon = weaponHolder.getCurrentGun();
            if (currWeapon != null && currWeapon.isShooting()) // + CHECK IF JOEL IS CURRENTLY FIRING !!!
                playerIsFiring = true;
            else
                playerIsFiring = false;


            if ((playerInSightRange || playerIsFiring) && !isDistracted && !isHit && !isSpitting && !waiting) SpitOnPlayer();
        
        }


    }

    private bool isInFront()
    {
        Vector3 playerDirection = transform.position - player.position;
        float angle = Vector3.Angle(transform.forward, playerDirection);
        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270) return true;
        return false;
    }

    private bool isInLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = player.position - transform.position;
        if (Physics.Raycast(transform.position, direction, out hit, sightRange))
        {
            if (hit.transform == player)
                return true;
        }
        return false;

    }

    private void Patroling()
    {
        animator.speed = 1f;
        animator.SetBool("walking", true);
        if(!isDistracted)
        laser.laserHit = null;
        agent.speed = 0.5f;
        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
        else
            agent.SetDestination(walkPoint);

    }

    private void SearchWalkPoint()
    {
        if (!isDistracted)
        {
            
            walkPointTranslation = walkPointTranslation == 5 ? -5 : 5;

            if (randomDirection == 0)
                walkPoint = new Vector3(transform.position.x + walkPointTranslation, transform.position.y, transform.position.z);
            if (randomDirection == 1)
                walkPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + walkPointTranslation);
            walkPointSet = true;
        }
        else
        {
            walkPointSet = true;
        }
    }

    private void SpitOnPlayer()
    {
        if (!spitClip.isPlaying)
            spitClip.PlayOneShot(spitClip.clip);
        isSpitting = true;
        transform.LookAt(player);
        agent.SetDestination(transform.position);
        animator.SetBool("walking", false);
        animator.SetTrigger("spitting");
        if (isStunned)
        {
            animator.speed = 0.5f;
            Invoke(nameof(StartSpit), 3f);
        }

        else
        {
            animator.speed = 1f;
            Invoke(nameof(StartSpit), 1.5f);
        }
        laser.laserHit = player;
        spitPosition= player.position;
      
    }

    private void StartSpit()
    {
        isSpitting = false;
        waiting = true;
        Vector3 spawnPos = transform.position;
        spawnPos.y += 1.7f;
        Instantiate(acidBall,spawnPos,transform.rotation);
        Invoke(nameof(WaitingEnded), 5f);
    }
    private void WaitingEnded()
    {
        isSpitting = false;
        waiting = false;
    }

    public void TakeDamage(int damage)
    {
        animator.speed = 1f;
        animator.SetBool("walking", false);

        health -= damage;
        if (health <= 0)
        {
            Die();
            isDead = true;

        }
        else
        {
            isHit = true;
            agent.SetDestination(transform.position);
            animator.SetTrigger("damage");
            Invoke(nameof(DamageEnded), 2f);

        }

    }

    private void DamageEnded()
    {
        isHit = false;
        playerInSightRange = false;
    }

    public void Distract(Transform pipe)
    {
        
        isDistracted = true;
        walkPoint = pipe.position;
        agent.SetDestination(pipe.position);
        laser.laserHit = pipe;
        Invoke(nameof(DistractionEnded), 4f);
    }

    private void DistractionEnded()
    {
        walkPointSet = false;
        animator.speed = 1f;
        animator.SetBool("walking", false);
        isDistracted = false;
        laser.laserHit = null;
    }

    public void Stun()
    {
        isStunned = true;
        Invoke(nameof(StunEnded), 3f);
    }

    private void StunEnded()
    {
        isStunned = false;

    }

    private void Die()
    {
        if (!isDead)
        {
            dieClip.PlayOneShot(dieClip.clip);
            isDead = true;
            animator.speed = 1f;
            agent.SetDestination(transform.position);
            animator.SetTrigger("dying");
            //CALL A METHOD TO INSTANTIATE BILE !!!
            Destroy(gameObject, 4);
            GameObject instan = Instantiate(bile, transform);
            instan.transform.SetParent(null);
            healthComponent.rageMeterAdd(50);
            healthComponent.infectedIsKilled();

        }
    }

}


