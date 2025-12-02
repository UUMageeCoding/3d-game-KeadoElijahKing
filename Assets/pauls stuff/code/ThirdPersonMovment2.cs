using UnityEngine;

public class ThirdPersonMovment2 : MonoBehaviour
{
  public CharacterController controller;

    public float speed = 6f;

    public float Gravity = -7f;
    [SerializeField] float JumpPower = 2f;
    private float height = 2;
    private float maxDist = 1;
    private Vector3 offsetHeight;
    private Vector3 spherePos;

    Vector3 velocity; 

    public float turnSmoothTime = 0.1f;
    float turnSmnoothVelocity;

    bool isGrounded;
    public float groundedCheckDistance;
    private float bufferCheckDistance = 0.1f;

    
    
    bool isJumping;

    public Transform cam;

     public Animator anim;

    void Start()
    {
        CharacterController controller = GetComponent<CharacterController>();
        offsetHeight = new Vector3(0.0f, height/2, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (!isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
            }
                velocity.y += Gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
        else if (isJumping)
        {
             isGrounded = false;
            velocity.y = 0;
            velocity.y +=   JumpPower;

            controller.Move(velocity * Time.deltaTime);
        }
        
         groundedCheckDistance = (GetComponent<CapsuleCollider>().height /2) + bufferCheckDistance;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * 3, ForceMode.Impulse);
            isJumping = true;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundedCheckDistance))
        {
            isGrounded = true;
             anim.SetBool("isJumping", true);
        }
        else
        {
            isGrounded = false;
             anim.SetBool("isJumping", false);
        }

        //Jump
        /*if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }*/

        if (direction.magnitude >= 0.1f)
        {
            float targetrAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetrAngle, ref turnSmnoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moverDir = Quaternion.Euler(0f, targetrAngle, 0f) * Vector3.forward;
            controller.Move(moverDir.normalized * speed * Time.deltaTime);

            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
        }
    }
}
