using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float maxVelocityChange = 2f;
    [SerializeField]
    float tiltAmount = 10f;
    public Joystick joystick;
    private Vector3 velocityVector = Vector3.zero; // initial velocity
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float _xMovementInput = joystick.Horizontal;
        float _yMovementInput = joystick.Vertical;
        // Calculate Velocity vector

        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _yMovementInput;

        // Calculating final movement vector

        Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;
        // apply movement
        Move(_movementVelocityVector);

        // apply rotation 

        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -1 * joystick.Horizontal * speed * tiltAmount);
    }

    private void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }


    // we'll using the rigid body  
    // we'll use fixed update method
    // as it work each physics statement
    private void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            // getting rigidbody current velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            // applying a force to reach the change in velocityy

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0f;
            rb.AddForce(velocityChange, ForceMode.Acceleration);
        }
    }
}
