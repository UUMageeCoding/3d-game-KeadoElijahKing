using TMPro;
using UnityEngine;

public class GroundCheckRaycast : MonoBehaviour
{
    public bool isGrounded = false;
    public float groundedCheckDistance;
    private float bufferCheckDistance = 0.1f;

    

    // Update is called once per frame
    void Update()
    {
        groundedCheckDistance = (GetComponent<CapsuleCollider>().height /2) + bufferCheckDistance;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(transform.up * 3, ForceMode.Impulse);
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundedCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
