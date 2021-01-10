using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NormalLogic : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    Animator animator;
    Laser laser;
   
    //Patroling
    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointTranslation = 5f;
    int randomDirection;

    //Attacking
    bool alreadyAttacked = false;
    float timeBetweenAttacks = 5f;

    //States
    float sightRange = 10f;
    float attackRange = 0.8f;
    float firingRange = 20f;
    bool playerInSightRange, playerInAttackRange, inFiringRange;

    //Health
    int health = 50;
    bool isDead = false;
    bool isHit = false;
    
    private void Start()
    {
        randomDirection = Random.Range(0, 2);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Joel").transform;
        animator = GetComponent<Animator>();
        laser = GetComponent<Laser>();
        SearchWalkPoint();
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
              
                if (!playerInSightRange && !playerInAttackRange && !isHit)
                {
                    if (walkPointSet) Patroling();
                    else SearchWalkPoint();

                }

                //Check for sight and attack range
                if (Vector3.Distance(player.position, transform.position) <= sightRange)
                {
                   
                    transform.LookAt(player);
                    playerInSightRange = true;

                    if (Vector3.Distance(player.position, transform.position) <= attackRange)
                        playerInAttackRange = true;

                    else
                        playerInAttackRange = false;

                }
           
                if (playerInSightRange && !playerInAttackRange && !isHit) ChasePlayer();
                if (playerInSightRange && playerInAttackRange && !isHit) AttackPlayer();
            
            }
        

        }

        private void Patroling()
    {
        animator.SetBool("walking", true);
        agent.speed = 0.5f;

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
        
        if(randomDirection == 0)
            walkPoint = new Vector3(transform.position.x + walkPointTranslation, transform.position.y, transform.position.z);
        if(randomDirection == 1)
            walkPoint = new Vector3(transform.position.x , transform.position.y, transform.position.z + walkPointTranslation);
            walkPointSet = true;
    }

    private void ChasePlayer()
    {

        animator.SetBool("walking", false);
        animator.SetBool("chasing", true);
        agent.speed = 2f;
        agent.SetDestination(player.position);
        laser.laserHit = player;

    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        animator.SetBool("chasing", false);
        if (!alreadyAttacked)
        {
            animator.SetTrigger("attacking");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
            
        
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void TakeDamage(int damage)
    {
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
    private void Die()
    {
        agent.SetDestination(transform.position);
        animator.SetTrigger("dying");

    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        if (other.gameObject.CompareTag("Tactical Shotgun")) TakeDamage(25);
        if (other.gameObject.CompareTag("Assault Rifle"))  TakeDamage(33);
        if (other.gameObject.CompareTag("Hunting Rifle")) TakeDamage (90);
        if (other.gameObject.CompareTag("Submachine Gun")) TakeDamage(20);
        if (other.gameObject.CompareTag("Pistol")) TakeDamage(36);




    }
}


