using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{

    public Transform Orientation;

    float HorizontalInput;
    float VerticalInput;


    AudioSource Ad;
    public AudioClip Jump, BGM, Run;



    Rigidbody rb;
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
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    bool IsBGM = false;


    // Start is called before the first frame update
    void Start()
    {


        Ad = GetComponent<AudioSource>();
        rb= GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        controller = GetComponent<CharacterController>();
        Ad.PlayOneShot(BGM);

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

        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");


        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

            playerSpeed = 5f;
            anim.SetBool("IsSprint", true);

          
            
        }
        else
        {
            playerSpeed = 2f;
            anim.SetBool("IsSprint", false);
        }

        Vector3 move = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;
        controller.Move(move * Time.deltaTime * playerSpeed);
       

        Vector3 checkWalk = new Vector3(move.x , 0 , move.z);
        anim.SetBool("IsWalk", checkWalk.normalized.magnitude > 0);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Ad.PlayOneShot(Jump);
        }

        anim.SetBool("IsJump", !groundedPlayer);
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


    }

    public void RUNN() 
    {

    }
    public void ENDRUNN() 
    {
        
    }

}

