using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimationController : MonoBehaviour
{
    Animator animator;
    MovementController movementController;
    int IsRunningHash;
    private void Awake()
    {
        IsRunningHash = Animator.StringToHash("IsRunning");
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();

    }

    void Update()
    {
        bool IsRunning = animator.GetBool(IsRunningHash);

        if (movementController.JoystickMoving && !IsRunning)
            animator.SetBool(IsRunningHash, true);
        else if (!movementController.JoystickMoving && IsRunning)
            animator.SetBool(IsRunningHash, false);

    }
}
