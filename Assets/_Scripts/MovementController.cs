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
    public bool JoystickMoving { get; set; }
    Vector3 currentMovement;
    GameState currentState;
    Rigidbody rb;
    public bool IsInMenu;
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }




    void Start()
    {
        Init();

    }
    private void OnEnable()
    {
        GameManager.OnGameStateChanged += SlowDownTruck;
        GameManager.OnGameStateChanged += ToggleJoyStickInput;
    }
    private void OnDisable()
    {
        GameManager.OnGameStateChanged += SlowDownTruck;
        GameManager.OnGameStateChanged -= ToggleJoyStickInput;

    }
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {

        JoystickMoving = joystick.Horizontal != 0 || joystick.Vertical != 0;
        HandleMovement();
        HandleRotation();

    }

    public void HandleMovement()
    {
        if (IsInMenu)
            currentMovement = new(0, 0, 0);
        else
            currentMovement = new(joystick.Horizontal, 0, joystick.Vertical);
        rb.velocity = movementSpeed * Time.deltaTime * currentMovement;

    }
    void HandleRotation()
    {
        Vector3 moveDirection;
        if (IsInMenu)
            moveDirection = new(0, 0, 0);
        else
            moveDirection = new(joystick.Horizontal, 0, joystick.Vertical);

        moveDirection.Normalize();

        if (moveDirection.sqrMagnitude > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            float step = rotationSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, step);

        }

    }

    void SlowDownTruck(GameState state)
    {
        if (state == GameState.CuttingGrass)
            movementSpeed = 250;
        else if (state == GameState.NotCuttingGrass)
            movementSpeed=Mathf.Lerp(250, 500, 1);
            //movementSpeed = 500;
    }
    void ToggleJoyStickInput(GameState state)
    {
        currentState = state;
        if ((state == GameState.Upgrading) || (state == GameState.InShop) || (state == GameState.InFarmUpgrade))
        {
            IsInMenu = true;
        }
        else
            IsInMenu = false;

    }

}
