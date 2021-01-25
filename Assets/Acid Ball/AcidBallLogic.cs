using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBallLogic : MonoBehaviour
{
   
    Vector3 spitPos;
    Rigidbody rb;
    Transform player;
    playerHealth healthComponent;
    WeaponSwitching weaponHolder;
    Vector3 spitDir;
    Vector3 scaleChange;
    public LayerMask ground;
    Vector3 floatingPos;
    bool collidedWithGround = false;
   
    // Start is called before the first frame update
    void Start()
    {
        spitPos = GameObject.FindGameObjectWithTag("Spitter").GetComponent<SpitterLogic>().spitPosition;
        rb = GetComponent<Rigidbody>();
        GameObject joel = GameObject.FindGameObjectWithTag("Joel");
        player = joel.transform;
        healthComponent = joel.GetComponent<playerHealth>();
        weaponHolder = player.GetComponentInChildren<WeaponSwitching>();
        scaleChange = new Vector3(0.5f, 0f, 0.5f);
        StartSpit();
    }

    // Update is called once per frame
    void Update()
    {
        if (ReachedGround() && !collidedWithGround)
        {
            collidedWithGround = true;
            rb.velocity = Vector3.zero;
            transform.position = floatingPos;
        }
        if(collidedWithGround)
            ExpandSpit();

        if (transform.localScale.magnitude > 10)
            Destroy(gameObject);





    }

    private bool ReachedGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.03f, ground))
        {

            floatingPos = hit.point;
            return true;

        }
        return false;
    }

    private void StartSpit()
    {
        
        spitDir = spitPos - transform.position;
        rb.AddForce(spitDir, ForceMode.Impulse);
    }

    private void ExpandSpit()
    {   
        transform.localScale += scaleChange * Time.deltaTime; 
    }
    private void ApplyDamage()
    {
        /// APPLY 20 POINTS OF DAMAGE ON JOEL HERE !!!
        healthComponent.applyDamage(20);
    }
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Joel"))
        {
            InvokeRepeating(nameof(ApplyDamage), 0f, 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Joel"))
        {
            CancelInvoke(nameof(ApplyDamage));
        }
    }
}
