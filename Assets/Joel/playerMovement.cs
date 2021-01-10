using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shootingClip;
    public AudioClip reloadClip;

    public CharacterController controller;
    public float speed = 12f;

    public Animator animator;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
            speed = 4f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.SetBool("run", false);
            speed = 2f;
        }


        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded)
        {

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("jump", true);
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("jump", false);

        }

        //hit Reaction
        if (Input.GetKeyDown(KeyCode.H))
        {
            animator.SetBool("hit", true);

        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            animator.SetBool("hit", false);

        }

        //hit Reaction
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetBool("dead", true);

        }
        


        //reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("reload", true);
            audioSource.PlayOneShot(reloadClip);

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            animator.SetBool("reload", false);
        }

        //fire
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("fire", true);
            audioSource.clip = shootingClip;
            audioSource.loop = true;
            audioSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("fire", false);
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
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

        
    }
}
