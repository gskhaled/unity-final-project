using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class playerMovement : MonoBehaviour

{

    bool walkingSoundBool = false;

    bool runningSoundBool = false;



    bool foundPrevious = false;



    //public GameObject player;

    public AudioSource audioSource;

    public AudioClip walking;

    public AudioClip running;
    public AudioClip alert;




    public CharacterController controller;

    public float speed = 2f;



    public Animator animator;



    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    bool isGrounded;



    // Start is called before the first frame update

    void Start()

    {

        speed = 2f;

        walkingSoundBool = false;

    }



    // Update is called once per frame

    void Update()

    {



        if (!animator.GetCurrentAnimatorStateInfo(3).IsName("dying"))

        {

            float x = Input.GetAxis("Horizontal");

            float z = Input.GetAxis("Vertical");



            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);



            //vertical

            if (z == 1)

            {

                animator.SetBool("forward", true);

            }

            if (z == 0)

            {

                animator.SetBool("forward", false);

                animator.SetBool("backward", false);

            }

            if (z == -1)

            {

                animator.SetBool("backward", true);

            }



            //horizontal

            if (x == 1)

            {



                animator.SetBool("right", true);

            }

            if (x == 0)

            {

                animator.SetBool("right", false);

                animator.SetBool("left", false);

            }

            if (x == -1)

            {

                animator.SetBool("left", true);

            }



            //run

            if (Input.GetKeyDown(KeyCode.LeftShift))

            {

                animator.SetBool("run", true);

                speed = 2f * speed;
                if((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
                {
                    walkingSoundPlay(2);
                }

            }

            if (Input.GetKeyUp(KeyCode.LeftShift))

            {

                animator.SetBool("run", false);

                speed = speed / 2f;

                walkingSoundStop(2);

            }





            //jump

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);



            if (!isGrounded)

            {

                Vector3 moveH = new Vector3(0, -2f, 0);

                controller.Move(moveH * Time.deltaTime);

            }

            if (Input.GetKeyDown(KeyCode.Space))

            {

                animator.SetBool("jump", true);

            }

            if (Input.GetKeyUp(KeyCode.Space))

            {

                animator.SetBool("jump", false);

            }

        }



        //Dead

        if (Input.GetKeyDown(KeyCode.L))

        {

            animator.SetBool("dead", true);



        }



        //reload

        if (Input.GetKeyDown(KeyCode.R))

        {

            animator.SetBool("reload", true);



        }

        if (Input.GetKeyUp(KeyCode.R))

        {

            animator.SetBool("reload", false);

        }



        //fire

        if (Input.GetKeyDown(KeyCode.Mouse0))

        {

            animator.SetBool("fire", true);

            /*    audioSource.clip = shootingClip;

                audioSource.loop = true;

                audioSource.Play();

            */
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))

        {

            animator.SetBool("fire", false);

            /*            audioSource.Stop();

                        audioSource.clip = null;

                        audioSource.loop = false;*/

        }



        //toss

        if (Input.GetKeyDown(KeyCode.Mouse1))

        {

            animator.SetBool("toss", true);

        }

        if (Input.GetKeyUp(KeyCode.Mouse1))

        {

            animator.SetBool("toss", false);

        }











        infectedAheadCheck();

        checkWalking();

    }



    void infectedAheadCheck()

    {

        GameObject[] normals = GameObject.FindGameObjectsWithTag("Normal");

        GameObject[] hunters = GameObject.FindGameObjectsWithTag("Hunter");

        GameObject[] chargers = GameObject.FindGameObjectsWithTag("Charger");

        GameObject[] spitters = GameObject.FindGameObjectsWithTag("Spitter");

        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");





        bool found = false;



        if (infectedAheadCheckHelper(normals))

        {

            found = true;

        }

        if (infectedAheadCheckHelper(hunters))

        {

            found = true;

        }

        if (infectedAheadCheckHelper(chargers))

        {

            found = true;

        }

        if (infectedAheadCheckHelper(spitters))

        {

            found = true;

        }

        if (infectedAheadCheckHelper(tanks))

        {

            found = true;

        }



        //play alert here

        if (found != foundPrevious)

        {

            
            this.audioSource.PlayOneShot(alert);

            Debug.Log(" INFECTRED ALERTTTT");

        }

        foundPrevious = found;

    }



    bool infectedAheadCheckHelper(GameObject[] infected)

    {

        foreach (GameObject inf in infected)

        {

            float dist = Vector3.Distance(this.transform.position, inf.transform.position);

            //distance to check

            if (dist < 2f)

            {

                return true;

                break;

            }

        }

        return false;

    }





    void checkWalking()

    {
        if ((Input.GetAxis("Vertical") == 0) && (Input.GetAxis("Horizontal") == 0))
        {
            walkingSoundStop(2);
            walkingSoundStop(1);
        }
        else
        {
            walkingSoundPlay(1);
        }
    }



    void walkingSoundPlay(int i)

    {

        if (i == 1)

        {
            if (!runningSoundBool)
            {
                if (!walkingSoundBool)

                {
                    Debug.Log("walkingsound");
                    this.GetComponent<AudioSource>().clip = walking;
                    this.GetComponent<AudioSource>().loop = true;
                    this.GetComponent<AudioSource>().Play();
                    walkingSoundBool = true;
                }
            }
        }

        if (i == 2)
        {
            if (!runningSoundBool)
            {
                walkingSoundBool = false;
                Debug.Log("runningsound");
                this.GetComponent<AudioSource>().clip = running;
                this.GetComponent<AudioSource>().loop = true;
                this.GetComponent<AudioSource>().Play();
                runningSoundBool = true;
            }
        }
    }



    void walkingSoundStop(int i)

    {

        if (i == 1)

        {

            if (walkingSoundBool)

            {

                Debug.Log("stopping walking sound");

                this.GetComponent<AudioSource>().clip = null;

                this.GetComponent<AudioSource>().loop = false;

                walkingSoundBool = false;

            }

        }

        if (i == 2)

        {

            if (runningSoundBool)

            {

                Debug.Log("stopping running sound");

                this.GetComponent<AudioSource>().clip = null;

                this.GetComponent<AudioSource>().loop = false;

                runningSoundBool = false;

                checkWalking();

                walkingSoundPlay(1);

            }

        }
    }
}