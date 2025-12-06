using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public class Enemy_AI : MonoBehaviour
{
    private enum AIState { Patrolling, Chasing, Attacking }

    [Header("State Machine")]
    [SerializeField] private AIState currentState = AIState.Patrolling;

    [Header("Patrol")]
    public float patrolSpeed = 2f;
    public float patrolRadius = 20f;
    public int patrolPointCount = 5;
    public Vector2 patrolWaitTimeRange = new Vector2(2f, 5f);
    public float waypointArrivalThreshold = 0.5f;
    private Vector3[] patrolPoints;
    private int currentWaypointIndex = 0;
    private bool isWaitingAtWaypoint = false;

    [Header("Line of Sight")]
    public LayerMask sightBlockingLayers;

    [Header("Ranges")]
    public float chaseRange = 10f;
    public float patrolRange = 15f;
    public float attackRange = 7f;
    public float chaseSpeed = 4f;

    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private float nextFireTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            enabled = false;
        }

        GeneratePatrolPoints();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            currentState = AIState.Chasing;
        }
        else
        {
            agent.isStopped = true;
            GoToNextWaypoint();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        switch (currentState)
        {
            case AIState.Patrolling:
                Patrol();
                break;
            case AIState.Chasing:
                Chase();
                break;
            case AIState.Attacking:
                Attack();
                break;
        }

        if (!agent.isStopped && agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector2 moveDirection = agent.velocity.normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (HasLineOfSightToPlayer(chaseRange))
        {
            CancelInvoke(nameof(GoToNextWaypoint));
            isWaitingAtWaypoint = false;
            currentState = AIState.Chasing;
            return;
        }

        if (!isWaitingAtWaypoint && !agent.pathPending && agent.remainingDistance <= waypointArrivalThreshold)
        {
            isWaitingAtWaypoint = true;
            float waitTime = Random.Range(patrolWaitTimeRange.x, patrolWaitTimeRange.y);
            Invoke(nameof(GoToNextWaypoint), waitTime);
            agent.isStopped = true;
        }
    }

    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < attackRange)
        {
            currentState = AIState.Attacking;
        }
        else if (distanceToPlayer > patrolRange)
        {
            currentState = AIState.Patrolling;
            GoToNextWaypoint();
        }
    }

    private void Attack()
    {
        agent.isStopped = true;

        if (!HasLineOfSightToPlayer(attackRange + 1f))
        {
            currentState = AIState.Chasing;
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Time.time >= nextFireTime)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void GoToNextWaypoint()
    {
        isWaitingAtWaypoint = false;
        if (patrolPoints.Length == 0) return;

        agent.isStopped = false;
        agent.destination = patrolPoints[currentWaypointIndex];
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolPoints.Length;
    }

    private void GeneratePatrolPoints()
    {
        patrolPoints = new Vector3[patrolPointCount];
        for (int i = 0; i < patrolPointCount; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
            {
                patrolPoints[i] = hit.position;
            } else {
                patrolPoints[i] = transform.position;
            }
        }
    }

    private bool HasLineOfSightToPlayer(float range)
    {
        if (playerTransform == null) return false;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > range) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, sightBlockingLayers);

        return hit.collider == null;
    }
}