using ControlFreak2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] FloatingJoystick joystick;
    CharacterController characterController;
    public bool JoystickMoving { get; set; }
    Vector3 currentMovement;
    Rigidbody rb;
    GameManager GM;
    GameState CurrentState;
    void Start()
    {
        Init();

    }

    private void Init()
    {
        GM = GameManager.Instance;
        CurrentState = GM.GetState();

        if (CurrentState == GameState.Tractor)
            rb = GetComponent<Rigidbody>();
        if (CurrentState == GameState.Farmer)
            characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        JoystickMoving = joystick.Horizontal != 0 || joystick.Vertical != 0;

    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    public void HandleMovement()
    {
        switch (CurrentState)
        {
            case (GameState.Tractor):

                if (JoystickMoving)
                {
                    currentMovement = new(joystick.Horizontal, 0, joystick.Vertical);
                    Vector3 velocity = movementSpeed * Time.deltaTime * new Vector3(joystick.Horizontal, 0, joystick.Vertical);
                    rb.velocity = velocity;
                }
                else
                    rb.velocity = Vector3.zero;
                break;
            case (GameState.Farmer):

                currentMovement = new(joystick.Horizontal, 0, joystick.Vertical);
                characterController.Move(movementSpeed * Time.deltaTime * currentMovement);
                break;


        }

    }

    void HandleRotation()
    {
        if (JoystickMoving)
        {
            Vector3 moveDirection = new(joystick.Horizontal, 0, joystick.Vertical);
            moveDirection.Normalize();

            if (moveDirection.sqrMagnitude > 0)
            {
                Quaternion lookRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                float step = rotationSpeed * Time.fixedDeltaTime;
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, step);

            }
        }
    }
}
