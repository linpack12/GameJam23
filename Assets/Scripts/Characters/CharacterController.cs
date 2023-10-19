using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Input for movement using keyboard inputs
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
            verticalInput = 1.0f;
        else if (Input.GetKey(KeyCode.S))
            verticalInput = -1.0f;

        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1.0f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1.0f;

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Calculate the desired movement force
        Vector3 movement = moveDirection * moveSpeed;
        rb.AddForce(movement, ForceMode.Force);

        // Rotate the character to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
