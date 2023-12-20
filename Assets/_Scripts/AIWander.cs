using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWander : MonoBehaviour
{
    public AnimalType animalType;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;
    private bool isWandering;
    private bool isIdleHeadTurn;
    [SerializeField] Transform targetPosition;

    public float wanderRadius = 10f;
    public float wanderInterval = 5f;
    private Coroutine wanderCoroutine;

    public float headTurnInterval = 10f;
    public float minHeadTurnTime = 2f;
    public float maxHeadTurnTime = 5f;

    private float nextHeadTurnTime;
    private float timeSinceDestinationSet = 4f;

    private void Start()
    {
        isWandering = false;
        isIdleHeadTurn = false;
        nextHeadTurnTime = Random.Range(minHeadTurnTime, maxHeadTurnTime);
        if (PlayerPrefs.GetInt($"FirstTimeWandering{animalType}") == 0)
            StartWandering();
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
    void StartWandering()
    {
        if (wanderCoroutine == null)
        {
            wanderCoroutine = StartCoroutine(Wander());
        }
    }
    void StopWandering()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
            isWandering = false;
        }
    }
    private IEnumerator Wander()
    {
        while (true)
        {

            if (!isWandering)
            {
                if (Time.time - timeSinceDestinationSet > wanderInterval)
                {
                    Vector3 randomPoint = GetRandomPointOnNavMesh(transform.position, wanderRadius);

                    if (Vector3.Distance(agent.transform.position, randomPoint) > agent.stoppingDistance)
                    {
                        agent.SetDestination(randomPoint);
                        timeSinceDestinationSet = Time.time; // Update time since the new destination was set
                        isWandering = true;
                        animator.SetBool("IsWalking", true);
                    }
                    else
                    {
                        animator.SetBool("IsWalking", false);
                    }
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

                if (Time.time - timeSinceDestinationSet > wanderInterval)
                {
                    agent.ResetPath();
                    isWandering = false;
                }
            }

            yield return null;
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
            PlayerPrefs.SetInt($"FirstTimeWandering{animal}", 1);
            StopWandering();
            animator.SetBool("IsWalking", true);
            StartCoroutine(MoveToTarget(animal));
        }
    }
    private IEnumerator MoveToTarget(AnimalType animal)
    {
        agent.SetDestination(targetPosition.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.1f);
        if (animal == AnimalType.Chicken)
            transform.localEulerAngles = new Vector3(180, 0, 180);
        else if (animal == AnimalType.Cow)
            transform.localEulerAngles = Vector3.zero;
        animator.SetBool("IsEating", true);
    }
    void BacktoWandering(AnimalType animal)
    {
        if (animal == animalType)
        {
            PlayerPrefs.SetInt($"FirstTimeWandering{animal}", 0);
            animator.SetBool("IsEating", false);
            StopCoroutine(MoveToTarget(animal));
            StartWandering();
        }
    }
}

