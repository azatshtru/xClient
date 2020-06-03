using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 6.0f;
    public float jumpForce = 1000f;
    public float gravityMultiplier = 1.4f;

    Rigidbody rb;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float _x = Input.GetAxisRaw("Horizontal");
        Vector3 movement = Vector3.right * _x * Time.fixedDeltaTime * playerSpeed;

        rb.MovePosition(rb.position + movement);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
            isGrounded = false;
        }

        if(isGrounded == false && rb.velocity.y < 1f)
        {
            rb.AddForce(Vector3.down * Mathf.Abs((rb.velocity.y) * gravityMultiplier), ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);
        Vector3 force = Vector3.up * jumpForce;
        rb.AddForce(force, ForceMode.Force);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
