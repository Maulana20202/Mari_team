using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float Speed;

    private Rigidbody rb;

    // Rotasi kecepatan penghalusan
    public float rotationSpeed = 5f;

    // Threshold untuk menghindari input kecil
    private float rotationThreshold = 0.1f;

    // Nyimpen data rotasi

    private Quaternion currentRotation;

    // Referensi kamera
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentRotation = this.transform.rotation;
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Input movement dari joystick
        float moveX = JoystickLeft.positionX;
        float moveZ = JoystickLeft.positionY;

        // Arah relatif terhadap kamera
        Vector3 moveDirection = cameraTransform.right * moveX + cameraTransform.forward * moveZ;

        // Pastikan tidak ada pergerakan pada sumbu Y
        moveDirection.y = 0f;

        Vector3 targetVelocity = moveDirection * moveSpeed;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.01f);

        if (moveDirection.magnitude > rotationThreshold)
        {
            moveDirection.Normalize();

            // Rotasi karakter mengikuti arah gerakan
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            currentRotation = transform.rotation;
        } else {
            transform.rotation = currentRotation;
        }

        Speed = rb.velocity.magnitude;
    }
}
