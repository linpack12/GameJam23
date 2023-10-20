using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float sprintMultiplier = 2.0f;
    public float driftForce = 10.0f;

    private Rigidbody dogRigidbody;
    private bool isSprinting = false;

    void Start()
    {
        dogRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // H�mta inmatning fr�n tangentbordet
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Skapa en r�relsevektor baserad p� inmatningen
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // R�kna ut hastigheten baserat p� om vi sprintar eller inte
        float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // R�r hunden
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        dogRigidbody.velocity = moveDirection * currentSpeed;

        // Implementera drifting
        if (isSprinting && (horizontalInput != 0 || verticalInput != 0))
        {
            Vector3 driftForceDirection = Vector3.Cross(moveDirection, Vector3.up);
            dogRigidbody.AddForce(driftForceDirection * driftForce, ForceMode.Force);
        }
    }
}