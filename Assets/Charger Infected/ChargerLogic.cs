using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargerLogic : MonoBehaviour
{
    public GameObject bile;

    NavMeshAgent agent;
    Transform player;
    Transform oldPlayer;
    playerHealth healthComponent;
    WeaponSwitching weaponHolder;

    Animator animator;
    Laser laser;
    AudioSource runClip;
    AudioSource dieClip;

    //Patroling
    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointTranslation = 5f;
    int randomDirection;

    //Charging
    bool isCharging = false;
    bool firstTime = true;
    float timeBetweenCharges = 5f;


    //States
    float sightRange = 10f;
    float firingRange = 20f;
    float attackRange = 1f;
    bool playerInSightRange, playerIsFiring;
    bool isDead = false;
    bool isHit = false;
    bool isDistracted = false;
    bool isStunned = false;

    //Health 
    int health = 600;

    //Joel 
    GameObject playerScript;

    private void Start()
    {
        randomDirection = Random.Range(0, 2);
        agent = GetComponent<NavMeshAgent>();
        GameObject joel = GameObject.FindGameObjectWithTag("Joel");
        player = joel.transform;
        healthComponent = joel.GetComponent<playerHealth>();
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();

        animator = GetComponent<Animator>();
        laser = GetComponent<Laser>();
        runClip = transform.GetChild(2).GetComponent<AudioSource>();
        dieClip = transform.GetChild(3).GetComponent<AudioSource>();
        // playerScript = GameObject.FindGameObjectWithTag("Joel").GetComponent<playerHealth>();
        SearchWalkPoint();
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject joel = GameObject.FindGameObjectWithTag("Joel");
        player = joel.transform;
        healthComponent = joel.GetComponent<playerHealth>();
    }

    private void Update()
    {
        if (!isDead)
        {

            if (!playerInSightRange && !isDistracted && !isHit && !isCharging)
            {
                if (walkPointSet) Patroling();
                else SearchWalkPoint();

            }

            //Check for sight, attack, and firing ranges

            if (isInLineOfSight() && isInFront() && !isDistracted && !isCharging)
            {

                Vector3 distVector = player.position - transform.position;
            //    distVector.y = 0;
                Quaternion angle = Quaternion.LookRotation(distVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * agent.speed);
                playerInSightRange = true;

            }
            else playerInSightRange = false;

            Gun currWeapon = weaponHolder.getCurrentGun();
            if (currWeapon != null && currWeapon.isShooting()) // + CHECK IF JOEL IS CURRENTLY FIRING !!!
                playerIsFiring = true;
  /*          else
                  playerIsFiring = false;*/


            if ((playerInSightRange || playerIsFiring) && !isDistracted && !isHit && !isCharging) ChargeAtPlayer();
            else if (!agent.hasPath && !isDistracted && !isHit && isCharging) CheckCharge();
            else if (!agent.hasPath && isDistracted) StayIdle();
               
      

        }

    }

    private void StayIdle()
    {
        animator.speed = 1f;
        animator.SetBool("charging", false);
        animator.SetBool("walking", false);
        animator.SetBool("idle", true);

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
        animator.SetBool("charging", false);
        animator.SetBool("idle", false);
        animator.SetBool("walking", true);
        laser.laserHit = null;
        agent.speed = 1f;
        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;

        else
            agent.SetDestination(walkPoint);



    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        // float randomZ = Random.Range(-walkPointRange, walkPointRange);
        walkPointTranslation = walkPointTranslation == 5 ? -5 : 5;

        if (randomDirection == 0)
            walkPoint = new Vector3(transform.position.x + walkPointTranslation, transform.position.y, transform.position.z);
        else
            walkPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + walkPointTranslation);
        walkPointSet = true;
    }

    private void ChargeAtPlayer()
    {
        if (!runClip.isPlaying)
            runClip.PlayOneShot(runClip.clip);

        isCharging = true;
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);
        animator.SetBool("charging", true);
        if (isStunned)
        {
           agent.speed = 5f;
           animator.speed = 0.5f;
        }

        else
        {
            agent.speed = 10f;
            animator.speed = 1f;
        }
       laser.laserHit = player;
       agent.SetDestination(player.position);

    }

    private void CheckCharge()
    {
        laser.laserHit = null;
        agent.SetDestination(transform.position);
        if (firstTime)
        {
            firstTime = false;
            Invoke(nameof(ResetCharge), timeBetweenCharges);
        }

        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            ///PIN DOWN JOEL !!! 
            healthComponent.pinDown();
            /// APPLY DAMAGE ON JOEL
            healthComponent.applyDamage(75);

            if (walkPointSet) Patroling();
            else SearchWalkPoint();

        }
        else
        {
            StayIdle();
        }
      
    }

    private void ResetCharge()
    {
        isCharging = false;
        firstTime = true;
    }


    public void TakeDamage(int damage)
    {
        animator.speed = 1f;
        animator.SetBool("walking", false);
        animator.SetBool("charging", false);
        animator.SetBool("idle", false);
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            Die();
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
        playerInSightRange = true;
    }

    public void Distract(Transform pipe)
    {
        agent.speed = 10f;
        animator.speed = 1f;
        isDistracted = true;
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);
        animator.SetBool("charging", true);
        agent.SetDestination(pipe.position);
        laser.laserHit = pipe;
        
    }

    private void DistractionEnded()
    {
        animator.speed = 1f;
        animator.SetBool("charging", false);
        isDistracted = false;
        laser.laserHit = null;
        Invoke(nameof(DistractionEnded), 4f);
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
            animator.speed = 3f;
            agent.SetDestination(transform.position);
            animator.SetTrigger("dying");
            //CALL A METHOD TO INSTANTIATE BILE !!!
            Destroy(gameObject, 2);
            GameObject instan = Instantiate(bile, transform);
            instan.transform.SetParent(null);
            healthComponent.rageMeterAdd(50);
        }

    }



}

  




