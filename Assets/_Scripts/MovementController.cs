using DG.Tweening;
using PT.Garden;
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
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SlowDownTruck;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged += SlowDownTruck;

    }
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
        GM = GameManager.Instance;
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
        currentMovement = new(joystick.Horizontal, 0, joystick.Vertical);
        rb.velocity = movementSpeed * Time.deltaTime * currentMovement;
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

    void SlowDownTruck(GameState state)
    {
        if (state == GameState.CuttingGrass)
            movementSpeed = 250;
        else if (state == GameState.NotCuttingGrass)
            movementSpeed = 500;
    }

}
