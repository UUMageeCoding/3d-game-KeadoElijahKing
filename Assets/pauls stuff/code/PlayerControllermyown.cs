using System;
using System.Collections; 
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControllermyown : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb; 
    public Transform cam;

    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Animator anim;

     void Start()
     {
        rb = GetComponent<Rigidbody>();
     }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
           
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded == true)
         {
             rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
             isGrounded = false;
         }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;  
        }
    }
}
