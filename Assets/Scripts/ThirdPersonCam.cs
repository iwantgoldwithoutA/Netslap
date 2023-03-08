using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform Orientation;
    public Transform Players;
    public Transform PlayerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewDir = Players.position - new Vector3(transform.position.x, Players.position.y, transform.position.z);
        Orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = Orientation.forward * verticalInput + Orientation.right * horizontalInput;

        if (inputDir != Vector3.zero) 
        
        {
            PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.fixedDeltaTime * rotationSpeed);
        }
    }
}
