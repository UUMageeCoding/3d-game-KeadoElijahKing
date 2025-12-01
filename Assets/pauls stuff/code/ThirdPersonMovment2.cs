using UnityEngine;

public class ThirdPersonMovment2 : MonoBehaviour
{
  public CharacterController controller;

    public float speed = 6f;

    public float Gravity = -7f;
    [SerializeField] float JumpPower = 2f;

    Vector3 velocity; 

    public float turnSmoothTime = 0.1f;
    float turnSmnoothVelocity;

    bool isGrounded;
    bool isJumping;

    public Transform cam;

     public Animator anim;

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

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

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
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
        }
    }
}
