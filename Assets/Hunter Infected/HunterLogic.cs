using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HunterLogic : MonoBehaviour
{
    public GameObject bile;

    NavMeshAgent agent;
    Transform player;
    Transform head;
    Transform distractPos;
    Animator animator;
    Rigidbody rb;
    Laser laser;
    AudioSource leapClip;
    AudioSource dieClip;
    playerHealth healthComponent;
    WeaponSwitching weaponHolder;

    //Patroling
    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointTranslation = 5f;
    int randomDirection;

    //Leaping
    bool firstTime = true;
    bool collided = false;
    bool isLeaping = false;
    float timer = 0f;
    float duration = 1f;


    //States
    float sightRange = 10f;
    float firingRange = 20f;
    bool playerInSightRange, playerInAttackRange, playerIsFiring;
    bool isDead = false;
    bool isHit = false;
    bool isDistracted = false;
    bool isStunned = false;


    //Health 
    int health = 250;

    //Joel 
    GameObject playerScript;
    
    private void Start()
    {
        randomDirection = Random.Range(0, 2);
        agent = GetComponent<NavMeshAgent>();
        rb = transform.gameObject.GetComponent<Rigidbody>();
        GameObject joel = GameObject.FindGameObjectWithTag("Joel");
        player = joel.transform;
        healthComponent = joel.GetComponent<playerHealth>();
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();
        head = GameObject.FindGameObjectWithTag("Head").transform;
        animator = GetComponent<Animator>();
        laser = GetComponent<Laser>();
        leapClip = transform.GetChild(2).GetComponent<AudioSource>();
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
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();
        head = GameObject.FindGameObjectWithTag("Head").transform;
    }

    private void Update()
    {
        if (!isDead)
        {

            if (!playerInSightRange && !isDistracted && !isHit && !isLeaping)
            {
                if (walkPointSet) Patroling();
                else SearchWalkPoint();

            }

            //Check for sight, attack, and firing ranges

            if (isInLineOfSight() && isInFront() && !isDistracted)
            {
                Vector3 distVector = player.position - transform.position;
                distVector.y = 0;
                Quaternion angle = Quaternion.LookRotation(distVector);
                transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * agent.speed);

                playerInSightRange = true;
            }
            else
            {
                if (!isDistracted)
                {
                    playerInSightRange = false;
                    isLeaping = false;
                    firstTime = true;
                    timer = 0f;
                    CancelInvoke(nameof(FirstTimeEnded));
                }
                
            }


            if (Vector3.Distance(player.position, transform.position) <= firingRange) // + CHECK IF JOEL IS CURRENTLY FIRING !!!
            {
                Gun currWeapon = weaponHolder.getCurrentGun();
                if (currWeapon != null && currWeapon.isShooting()) // + CHECK IF JOEL IS CURRENTLY FIRING !!!
                    playerIsFiring = true;
            }
            else
                playerIsFiring = false;

            if ((playerInSightRange || playerIsFiring) && !isDistracted && !isHit && !collided)
            {
                if (firstTime)
                {
                    isLeaping = true;
                    StartLeap(player);
                }
                else LeapAttack(head);
            }
            if (isDistracted)
            {
                if (firstTime)
                {
                    isLeaping = true;
                    StartLeap(distractPos);
                }
                else LeapAttack(distractPos);
            }

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
        animator.SetBool("leaping", false);
        animator.SetBool("walking", true);
        laser.laserHit = null;
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

        if (randomDirection == 0)
            walkPoint = new Vector3(transform.position.x + walkPointTranslation, transform.position.y, transform.position.z);
        if (randomDirection == 1)
            walkPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + walkPointTranslation);
        walkPointSet = true;
    }

    private void StartLeap(Transform leapPos)
    {
        if (firstTime)
        {
           
            agent.SetDestination(transform.position);
            animator.SetBool("walking", false);
            animator.SetBool("leaping", true);
            transform.LookAt(leapPos);
            laser.laserHit = leapPos;

              if (isStunned)
                {
                    animator.speed = 0.5f;
                    Invoke(nameof(FirstTimeEnded), 2f);
                }
                else
                {
                    animator.speed = 1f;
                    Invoke(nameof(FirstTimeEnded), 1f);
                }
            
        }
    }
    private void FirstTimeEnded()
    {
        firstTime = false;
    }
    private void LeapAttack(Transform leapPos)
    {
        if(!leapClip.isPlaying)
            leapClip.PlayOneShot(leapClip.clip);
        transform.LookAt(leapPos);
        Vector3 center = (agent.transform.position + leapPos.position) * 0.5f;
        center -= Vector3.up;


        Vector3 agentRelCenter = agent.transform.position - center;
        Vector3 playerRelCenter = leapPos.position - center;

        timer += Time.deltaTime;
       

            if (isStunned)
                duration = 1.5f;
            else
                duration = 0.5f;
        
            
        float fracComplete = timer / duration;
        agent.transform.position = Vector3.Slerp(agentRelCenter, playerRelCenter, fracComplete);
        
        agent.transform.position += center;

    }
 
    private void AttackPlayer()
    {
       
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        animator.SetBool("leaping", false);
        animator.SetBool("walking", false);
        animator.SetTrigger("attacking");

        if (isStunned) animator.speed = 0.5f;
        else animator.speed = 1f;


    }


    public void TakeDamage(int damage)
    {
        animator.speed = 1f;
        animator.SetBool("walking", false);
        animator.SetBool("leaping", false);
        CancelInvoke(nameof(ApplyDamage));
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
        Invoke(nameof(ResetLeap), 5f);
        
    }
    private void ResetLeap()
    {
        collided = false;
        isLeaping = false;
        firstTime = true;
        timer = 0f;
    }

    public void Distract(Transform pipe)
    {
        distractPos = pipe;
        animator.speed = 1f;
        timer = 0f;
        isDistracted = true;
        Invoke(nameof(DistractionEnded), 4f);
    }
    
    private void DistractionEnded()
    {
        animator.speed = 1f;
        timer = 0f;
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
            CancelInvoke(nameof(ApplyDamage));
            agent.SetDestination(transform.position);
            animator.SetTrigger("dying");
            //CALL A METHOD TO INSTANTIATE BILE !!!
            Destroy(gameObject, 2);
            GameObject instan = Instantiate(bile, transform);
            instan.transform.SetParent(null);
            healthComponent.rageMeterAdd(50);
        }
    }

    private void ApplyDamage()
    {

        ///Apply damage on Joel here !!!
        healthComponent.applyDamage(10);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Joel") && !isDead && !isHit && !isDistracted && isLeaping)
        {
            collided = true;
            isLeaping = false;
            firstTime = true;
            //PIN DOWN JOEL !!!
            healthComponent.pinDownHold();

            InvokeRepeating(nameof(ApplyDamage), 0, 1);
            AttackPlayer();
        }
    }

  
}


