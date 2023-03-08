using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class SlapControl : MonoBehaviour
{
    // Start is called before the first frame update

    public static SlapControl Instance;

    AudioSource Ad;
    public AudioClip Yaheuy;

    public void Start()
    {
        Ad = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        Instance = this;
    }

    public GameObject Legs;
    public Animator Anim;
    public CapsuleCollider col;
    public int Speed;

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Anim.SetBool("IsFire", true);
        }
        else 
        {
            Anim.SetBool("IsFire", false);
        }
    }

    public void EnableCol()
    {
        col.enabled = true;
        Ad.PlayOneShot(Yaheuy);
    }

    public void DisableCol()
    {
        col.enabled = false;
    }

    public void SlapReturn(GameObject other)
    {
        
        other.gameObject.GetComponent<Rigidbody>().AddForce(Speed * transform.forward , ForceMode.Impulse);
        
    }
}

