using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    private Vector3 currentVelocity = Vector3.zero;
    private Rigidbody rb;
    private GameObject manCollider;
    private GameObject dogCollider;
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

        // Calculate the desired movement velocity
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // Smoothly lerp the current velocity to the target velocity
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 0.2f);

        // Apply the velocity to the Rigidbody
        rb.velocity = currentVelocity;


        // Rotate the character to face the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(gameObject.CompareTag("dog")) {
            
            gameObject.transform.position = other.transform.position;
        }

        if(other.gameObject.CompareTag("moveable")) {
            
        }
    }
}

