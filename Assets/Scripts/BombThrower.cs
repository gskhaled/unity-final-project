using UnityEngine;

public class BombThrower : MonoBehaviour
{
    [System.Serializable]
    public class Bomb
    {
        public enum BombType
        {
            Molotov,
            Stun,
            Pipe
        }
        public BombType type;
        public GameObject bomb;
    }
    public Camera FPSCam;
    public Bomb[] bombs;
    public float throwForce = 40f;
    public int selectedBomb = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ThrowBomb();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (selectedBomb >= bombs.Length - 1)
                selectedBomb = 0;
            else
                selectedBomb++;
            Debug.Log(bombs[selectedBomb].bomb.name);
        }
    }

    void ThrowBomb()
    {
        GameObject toThrow = Instantiate(bombs[selectedBomb].bomb, FPSCam.transform.position, FPSCam.transform.rotation);
        Rigidbody rb = toThrow.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        rb.useGravity = true;
    }
}
