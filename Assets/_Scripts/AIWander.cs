using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AIWander : MonoBehaviour
{
    public AnimalType animalType;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isWandering;
    private bool isIdleHeadTurn;
    [SerializeField] Transform targetPosition;

    public float wanderRadius = 10f;
    public float wanderInterval = 5f;

    public float headTurnInterval = 10f;
    public float minHeadTurnTime = 2f;
    public float maxHeadTurnTime = 5f;

    private float nextHeadTurnTime;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        isWandering = false;
        isIdleHeadTurn = false;
        nextHeadTurnTime = Random.Range(minHeadTurnTime, maxHeadTurnTime);

        StartCoroutine(Wander());
    }
    private void OnEnable()
    {
        TroughStack.OnTroughFull += GoToTargetPosition;
        Timer.OnTimeOut += BacktoWandering;
    }
    private void OnDisable()
    {
        TroughStack.OnTroughFull -= GoToTargetPosition;
        Timer.OnTimeOut -= BacktoWandering;

    }

    private IEnumerator Wander()
    {
        while (true)
        {
            if (!isWandering)
            {
                Vector3 randomPoint = GetRandomPointOnNavMesh(transform.position, wanderRadius);

                if (Vector3.Distance(agent.transform.position, randomPoint) > agent.stoppingDistance)
                {
                    agent.SetDestination(randomPoint);
                    isWandering = true;
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                }
            }


            if (!isWandering)
            {
                if (Time.time >= nextHeadTurnTime)
                {
                    isIdleHeadTurn = true;
                    nextHeadTurnTime = Time.time + Random.Range(minHeadTurnTime, maxHeadTurnTime);
                }
                else
                {
                    isIdleHeadTurn = false;
                }
            }
            else
            {
                isIdleHeadTurn = false;
            }

            yield return new WaitForSeconds(wanderInterval);
        }
    }
    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);

        return hit.position;
    }
    private void Update()
    {
        if (isWandering && !agent.pathPending && agent.remainingDistance < 0.1f)
        {
            isWandering = false;
            animator.SetBool("IsWalking", false);
        }

        animator.SetBool("Turn", isIdleHeadTurn);
    }
    public void GoToTargetPosition(AnimalType animal)
    {
        if (animal == animalType)
        {
            //Debug.LogError("GOTOTARGETPOSITION" + gameObject.name);
            StopCoroutine(Wander());
            animator.SetBool("IsWalking", true);
            StartCoroutine(MoveToTarget(animal));
        }
    }
    private IEnumerator MoveToTarget(AnimalType animal)
    {
        agent.SetDestination(targetPosition.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.1f);
        if (animal == AnimalType.Chicken)
            transform.eulerAngles = new(0, 90, 0);
        else if (animal == AnimalType.Cow)
            transform.eulerAngles = new(0, 0, 0);
        animator.SetBool("IsEating", true);
    }
    void BacktoWandering(AnimalType animal)
    {
        if (animal == animalType)
        {
            //Debug.LogError("BacktoWandering" + gameObject.name);
            isWandering = false;
            animator.SetBool("IsEating", false);
            animator.SetBool("IsWalking", true);
            StopCoroutine(MoveToTarget(animal));
            StartCoroutine(Wander());
        }
    }
}

