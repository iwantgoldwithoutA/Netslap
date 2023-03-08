using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed;
    public bool ShiftPress;

    public Transform Orientation;

    float HorizontalInput;
    float VerticalInput;


    Vector3 moveDir;

    Rigidbody rb;
    public float playerHeight;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public KeyCode Space = KeyCode.Space;
    public KeyCode fire1 = KeyCode.Mouse0;


    [SerializeField]
    private Animator anim;

    [SerializeField]
    private GameObject camHolder , third;

    [SerializeField]
    private SlapControl slap;




    public LayerMask Ground;
    public bool grounded;

    // Start is called before the first frame update
    void Start()
    {



        rb= GetComponent<Rigidbody>();
        rb.freezeRotation = true;


        
    }

    // Update is called once per frame
    private void MyInput() 
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

       
    }

    private void MovePlayer() 
    
    {
        moveDir = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        else if (!grounded) 
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //Animation

        Vector3 anim_state = new Vector3(moveDir.x , 0 , moveDir.z);

        anim.SetBool("IsWalk" , anim_state.normalized.magnitude != 0);
           
    }
    void Update()
    {

        if (!isLocalPlayer)
        {
            camHolder.SetActive(false);
            third.SetActive(false);
            slap.enabled = false;
            this.enabled = false;
            return;
        }
        

        MyInput();


        SpeedCon();
        if (grounded)
        {
            rb.drag = groundDrag;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 5f;
                airMultiplier = 2.0f;
                anim.SetBool("IsSprint", true);
            }
            else
            {
                moveSpeed = 2f;
                airMultiplier = 1f;
                anim.SetBool("IsSprint", false);
            }

        }
        else 
        {
            rb.drag = 0;
        }



    }
    private void FixedUpdate()
    {

        MovePlayer();
       
           

        

        if (Input.GetKey(Space) && grounded)
        {

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            anim.SetBool("IsJump", true);


        }
        else
        {
            anim.SetBool("IsJump", !grounded);
        }


       
        


    }

    private void SpeedCon() 
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed) 
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            grounded = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

}

