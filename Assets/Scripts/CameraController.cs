using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // The player's transform
    public float distance = 10.0f;  // Distance from the player
    public float height = 5.0f;    // Height above the player
    public float smoothSpeed = 5.0f;  // Smoothing speed
    public float angle = 45.0f;

    private Vector3 offset;

    private void Start()
    {
        // Calculate the initial offset based on the fixed angle
        float angleInRadians = angle * Mathf.Deg2Rad; // Change the angle as needed
        offset = new Vector3(0, height, Mathf.Sin(angleInRadians) * distance * -1);
    }

    private void Update()
    {
        if (target == null)
            return;

        // Calculate the desired camera position
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Make the camera look at the player's position
        transform.LookAt(target.position);
    }
}
