using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class playerHealth : MonoBehaviour

{

    public AudioSource hitSound;

    public AudioSource rageSound;

    public AudioSource dyingSound;

    public AudioSource companionFire;
    


    public GameObject healthText;

    public GameObject collText;

    public Animator animator;



    int killedInfected;
    float lastRageAddition;

    int health;

    int collected = 0;

    int rageMeter = 0;

    float ragingMeterTimer = 0.0f;

    float rageTimer = 0.0f;

    bool raging = false;

    bool rageAvailable = false;

    private bool dead = false;

    bool pinHold = false;

    int rageMultiplier = 1;






    // Start is called before the first frame update

    void Start()

    {
        killedInfected = 0;
        health = 300;

        collected = 0;

        dead = false;

        bool rageAvailable = false;

    }



    // Update is called once per frame

    void Update()

    {

        if (!animator.GetCurrentAnimatorStateInfo(3).IsName("dying"))

        {



            //Exiting Animations when done

            if (animator.GetCurrentAnimatorStateInfo(2).IsName("pick_up"))

            {

                animator.SetBool("pick", false);

            }

            if (animator.GetCurrentAnimatorStateInfo(2).IsName("dodging"))

            {

                animator.SetBool("dodge", false);

            }

            if (animator.GetCurrentAnimatorStateInfo(3).IsName("hit_Reaction"))

            {

                animator.SetBool("hit", false);

            }

            if ((animator.GetCurrentAnimatorStateInfo(3).IsName("pin_Down")) && !pinHold)

            {

                animator.SetBool("pinned", false);

            }





            //checking health

            if (health <= 0)

            {

                animator.SetBool("dead", true);
                dyingSound.enabled = true;
                if (!dead)
                    dyingSound.Play();
                dead = true;

                //AudioClip dyingSound = GameObject.Find("dyingSound").GetComponent<AudioSource>().clip;

                //this.GetComponent<AudioSource>().PlayOneShot(dyingSound);

            }





            //testing

/*            if (Input.GetKeyDown(KeyCode.Alpha9))

            {

                applyDamage(10);

                //playPickup();

            }*/



            //Dodging

            bool moving = ( (Input.GetAxis("Vertical")!=0) || (Input.GetAxis("Horizontal")!=0) );

            if (Input.GetKeyDown(KeyCode.LeftControl)&&moving )
            {
                animator.SetBool("dodge", true);
            }


            //fireOrder
            if (Input.GetKeyDown(KeyCode.Q))

            {
                //AudioClip companionFire = GameObject.Find("fireOrder").GetComponent<AudioSource>().clip;
                companionFire.enabled = true;
                companionFire.PlayOneShot(companionFire.clip);

                //this.GetComponent<AudioSource>().PlayOneShot(companionFire);

            }







            //rage

            rage();



            //check cheats

            checkCheats();



        }

    }







    //checking collectibles

    private void OnControllerColliderHit(ControllerColliderHit hit)

    {

        if (Input.GetKey(KeyCode.E))

        {/*

            if (hit.gameObject.tag == "coll")

            {

                animator.SetBool("pick", true);

                collected++;

                collText.GetComponent<Text>().text = "collected:" + collected;

                Destroy(hit.gameObject);



            }



            if (hit.gameObject.tag == "healthPack")

            {

                animator.SetBool("pick", true);



                health += 50;

                if (health > 300)

                {

                    health = 300;

                }

                healthText.GetComponent<Text>().text = "health:" + health;

                Destroy(hit.gameObject);

            }
*/
        }



    }



    void rage()

    {

        ragingMeterTimer += Time.deltaTime;

        rageTimer += Time.deltaTime;

        int meterSeconds = (int)ragingMeterTimer % 60;

        int ragingSeconds = (int)rageTimer % 60;



        if (Time.time - lastRageAddition <= 3)

        {

            if (rageMeter >= 100)

            {

                rageAvailable = true;

            }

        }

        else

        {

            if (!rageAvailable && !raging)

            {

                rageMeter = 0;

                ragingMeterTimer = 0;

                collText.GetComponent<Text>().text = "RageMeter:" + rageMeter;

            }

        }







        if (raging && ragingSeconds > 7)

        {

            raging = false;
            rageAvailable = false;
            this.GetComponent<playerMovement>().speed = this.GetComponent<playerMovement>().speed / 2f;

            ragingMeterTimer = 0;

            rageMeter = 0;

            collText.GetComponent<Text>().text = "Rage Meter:" + rageMeter;

        }



        if (rageAvailable && Input.GetKeyDown(KeyCode.F))

        {

            if (!raging)

            {
                rageSound.enabled = true;
               // AudioClip rageSound = GameObject.Find("rageSound").GetComponent<AudioSource>().clip;
                rageSound.PlayOneShot(rageSound.clip);

               // this.GetComponent<AudioSource>().PlayOneShot(rageSound);

                this.GetComponent<playerMovement>().speed = 2f * this.GetComponent<playerMovement>().speed;

                rageTimer = 0;

                raging = true;

                rageAvailable = false;

                rageMeter = 0;

                collText.GetComponent<Text>().text = "RAGINGGG!!";



            }

        }

    }

    void checkCheats()

    {

        //add health

        if (Input.GetKeyDown(KeyCode.H))

        {

            health += 50;

            if (health > 300)

            {

                health = 300;

            }

        }

        //add 10 pts rage

        if (Input.GetKeyDown(KeyCode.M))

        {

            rageMeterAdd(10);

        }

        //toggle rage

        if (Input.GetKeyDown(KeyCode.Y))

        {

            if (!raging)

            {

                this.GetComponent<playerMovement>().speed = 2f * this.GetComponent<playerMovement>().speed;

                rageTimer = 0;

                raging = true;

                rageAvailable = false;

                rageMeter = 0;

                collText.GetComponent<Text>().text = "RAGINGGG!!";
                rageSound.enabled = true;
                rageSound.PlayOneShot(rageSound.clip);
                /*AudioClip rageSound = GameObject.Find("rageSound").GetComponent<AudioSource>().clip;

                this.GetComponent<AudioSource>().PlayOneShot(rageSound);*/

            }

        }

        if (Input.GetKeyDown(KeyCode.N))

        {

            if (raging)

            {

                raging = false;

                this.GetComponent<playerMovement>().speed = this.GetComponent<playerMovement>().speed / 2f;

                ragingMeterTimer = 0;

                //rageMeter = 0;

                collText.GetComponent<Text>().text = "Rage Meter:" + rageMeter;

            }

        }

    }



    //public methods



    //Applies input damage on joel

    public void applyDamage(int damage)

    {

        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("dodging"))

        {
            hitSound.enabled = true;
            hitSound.PlayOneShot(hitSound.clip);

            //this.GetComponent<AudioSource>().PlayOneShot(hitSound);

            health -= damage;

            healthText.GetComponent<Text>().text = "Health:" + health;

            animator.SetBool("hit", true);

        }

    }

    public void rageMeterAdd(int points)

    {
        lastRageAddition = Time.time;
        rageMeter += points*rageMultiplier;

        collText.GetComponent<Text>().text = "rageMeter:" + rageMeter;

    }

    public bool isDead()

    {

        return dead;

    }

    public bool isRaging()

    {

        return raging;

    }

    public int rageMeterNumber()

    {

        return rageMeter;

    }

    public int healthValue()

    {

        return health;

    }

    public void pinDown()

    {
        animator.SetBool("pinned", true);
    }
    public void pinDownHold()

    {
        animator.SetBool("pinned", true);
        pinHold = true;
    }
    public void pinDownCancel()
    {
        pinHold = false;
    }
    public bool isPinned()
    {
        return pinHold;
    }
    public void increaseHealth(int add)

    {

        health += add;

        if (health > 300)

        {

            health = 300;

        }

    }

    public void playPickup()

    {

        animator.SetBool("pick", true);

    }
    public void infectedIsKilled()
    {
        killedInfected += 1;
    }
    public int getTotalKilled()
    {
        return killedInfected;
    }

    public void setRageMultiplier(int multiplier)
    {
        rageMultiplier = multiplier;
    }

}