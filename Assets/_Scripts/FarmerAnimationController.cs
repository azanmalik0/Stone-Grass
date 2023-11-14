using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAnimationController : MonoBehaviour
{
    Animator animator;
    MovementController movementController;
    FarmerStack farmerStack;
    int IsRunningHash;
    private void Awake()
    {
        IsRunningHash = Animator.StringToHash("IsRunning");
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<MovementController>();
        farmerStack = GetComponentInChildren<FarmerStack>();

    }

    void Update()
    {
        bool IsRunning = animator.GetBool(IsRunningHash);
        if (farmerStack.transform.childCount > 0)
            animator.SetLayerWeight(1, 1);
        else
            animator.SetLayerWeight(1, 0);

        if (movementController.JoystickMoving && !IsRunning)
            animator.SetBool(IsRunningHash, true);
        else if (!movementController.JoystickMoving && IsRunning)
            animator.SetBool(IsRunningHash, false);

    }
}
