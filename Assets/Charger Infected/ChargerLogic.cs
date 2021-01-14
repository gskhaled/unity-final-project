using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChargerLogic : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Transform oldPlayer;
    Animator animator;
    Laser laser;
    AudioSource runClip;
    AudioSource dieClip;

    //Patroling
    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointTranslation = 7f;
    int randomDirection;

    //Charging
    bool chargeEnded = true;
    bool checkedCharge = false;
    bool pinningDown = false;
    float timeBetweenCharges = 5f;

    //States
    float sightRange = 10f;
    float attackRange = 2f;
    float firingRange = 20f;
    bool playerInSightRange, playerInAttackRange, playerIsFiring;
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
        player = GameObject.FindGameObjectWithTag("Joel").transform;
        animator = GetComponent<Animator>();
        laser = GetComponent<Laser>();
        runClip = transform.GetChild(2).GetComponent<AudioSource>();
        dieClip = transform.GetChild(3).GetComponent<AudioSource>();
        // playerScript = GameObject.FindGameObjectWithTag("Joel").GetComponent<playerHealth>();
        SearchWalkPoint();
        Stun();

    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Joel").transform;
    }

    private void Update()
    {
        if (!isDead)
        {
            
            if (!playerInSightRange  && !isDistracted && !isHit && chargeEnded)
            {
                if (walkPointSet) Patroling();
                else SearchWalkPoint();

            }

            //Check for sight, attack, and firing ranges

            if (isInLineOfSight() && isInFront() && chargeEnded)
            {
                
                        Vector3 distVector = player.position - transform.position;
                        distVector.y = 0;
                        Quaternion angle = Quaternion.LookRotation(distVector);
                        transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * agent.speed);
                        playerInSightRange = true;
            }
            else playerInSightRange = false;

            /*  if (Vector3.Distance(player.position, transform.position) <= firingRange) // + CHECK IF JOEL IS CURRENTLY FIRING !!!
                  playerIsFiring = true;
              else
                  playerIsFiring = false;*/


            if ((playerInSightRange || playerIsFiring) && !isDistracted && !isHit && chargeEnded) ChargeAtPlayer();
            else if (!agent.hasPath && !chargeEnded && !checkedCharge &&!pinningDown && !isDistracted && !isHit ) CheckCharge();
                

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
        chargeEnded = false;
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
        checkedCharge = true;
        agent.SetDestination(transform.position);
        animator.SetBool("charging", false);
        animator.SetBool("walking", false);
        if (pinningDown)
           animator.SetTrigger("attacking");
        else
           animator.SetBool("idle", true);
        Invoke(nameof(ResetCharge), timeBetweenCharges);
    }

    private void ResetCharge()
    {
        chargeEnded = true;
        checkedCharge = false;
        pinningDown = false;
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
        animator.speed = 1.5f;
        agent.speed = 3f;
        isDistracted = true;
        animator.SetBool("walking", false);
        animator.SetBool("idle", false);
        animator.SetBool("charging", true);
        agent.SetDestination(pipe.position);
        laser.laserHit = pipe;
        Invoke(nameof(DistractionEnded), 4f);
    }

    private void DistractionEnded()
    {
        animator.speed = 1f;
        animator.SetBool("charging", false);
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
        isDead = true;
        animator.speed = 1f;
        agent.SetDestination(transform.position);
        animator.SetTrigger("dying");
        //CALL A METHOD TO INSTANTIATE BILE !!!

    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Joel") && !pinningDown && !isDead && !isHit)
        {
            Debug.Log("collided");
            pinningDown = true;
            CheckCharge();
            /// PIN DOWN JOEL !!!
            /// APPLY 75 POINTS OF DAMAGE ON JOEL
           
        }

        
           
    }

  

}


