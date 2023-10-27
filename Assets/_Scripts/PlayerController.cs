using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] float playerSpeed;
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] float steerSpeed;
    Rigidbody rb;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        CheckAxis();
    }

    private void CheckAxis()
    {
        float inputZ = joystick.Horizontal;
        float inputX = -joystick.Vertical;

        if (inputX != 0 || inputZ != 0)
        {
            PlayerLookAndMove(inputX, inputZ);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void PlayerLookAndMove(float x, float z)
    {
        Vector3 moveDirection = new Vector3(x, 0, z);
        moveDirection.Normalize();

        if (moveDirection.sqrMagnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            float step = steerSpeed * Time.fixedDeltaTime;
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, step);

            Vector3 velocity = moveDirection * playerSpeed;
            rb.velocity = velocity;
        }
    }
}
