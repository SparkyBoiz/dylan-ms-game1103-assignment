using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles enemy AI, including movement towards the player and dealing damage on contact.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(NavMeshAgent))]
public class Enemy_AI : MonoBehaviour
{
    private enum AIState { Patrolling, Chasing, Attacking }

    [Header("State Machine")]
    [SerializeField] private AIState currentState = AIState.Patrolling;

    [Header("Patrol")]
    [Tooltip("The speed at which the enemy moves when patrolling.")]
    public float patrolSpeed = 2f;
    [Tooltip("The radius around the starting point in which the enemy will generate patrol waypoints.")]
    public float patrolRadius = 20f;
    [Tooltip("The number of random patrol points to generate.")]
    public int patrolPointCount = 5;
    [Tooltip("The range of time the enemy will wait at a patrol point before moving to the next one.")]
    public Vector2 patrolWaitTimeRange = new Vector2(2f, 5f);
    [Tooltip("How close the enemy needs to be to a waypoint to consider it 'reached'.")]
    public float waypointArrivalThreshold = 0.5f;
    private Vector3[] patrolPoints;
    private int currentWaypointIndex = 0;
    private bool isWaitingAtWaypoint = false;

    [Header("Line of Sight")]
    [Tooltip("Layers that will block the enemy's line of sight (e.g., Walls, Obstacles).")]
    public LayerMask sightBlockingLayers;

    [Header("Ranges")]
    [Tooltip("The distance at which the enemy will start chasing the player.")]
    public float chaseRange = 10f;
    [Tooltip("The distance at which the enemy will stop chasing and return to patrolling.")]
    public float patrolRange = 15f;
    [Tooltip("The distance at which the enemy will start attacking the player.")]
    public float attackRange = 7f;
    [Tooltip("The speed at which the enemy moves when chasing.")]
    public float chaseSpeed = 4f;

    [Header("Combat")]
    [Tooltip("The projectile prefab to be instantiated.")]
    public GameObject projectilePrefab;
    [Tooltip("The point from which projectiles are fired.")]
    public Transform firePoint;
    [Tooltip("The time in seconds between shots.")]
    public float fireRate = 1f;

    private NavMeshAgent agent;
    private Transform playerTransform;
    private float nextFireTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // The NavMeshAgent needs to be configured for 2D.
        // This is done by setting updateRotation and updateUpAxis to false.
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Enemy_AI: Player not found! Make sure the player has the 'Player' tag.", this);
            enabled = false;
        }

        GeneratePatrolPoints();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("Enemy_AI: Could not generate patrol points. Switching to Chase state.", this);
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

        // State machine logic
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

        // Make the enemy face the direction it's moving, but only if it's not stopped.
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

        // If player is in chase range, switch to chase state
        if (HasLineOfSightToPlayer(chaseRange))
        {
            CancelInvoke(nameof(GoToNextWaypoint)); // Cancel any pending patrol movement
            isWaitingAtWaypoint = false;
            currentState = AIState.Chasing;
            return;
        }

        // If the enemy has reached its waypoint, go to the next one
        if (!isWaitingAtWaypoint && !agent.pathPending && agent.remainingDistance <= waypointArrivalThreshold)
        {
            isWaitingAtWaypoint = true;
            // Wait for a random time before moving to the next point
            float waitTime = Random.Range(patrolWaitTimeRange.x, patrolWaitTimeRange.y);
            Invoke(nameof(GoToNextWaypoint), waitTime);
            agent.isStopped = true; // Stop while waiting
        }
    }

    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // If player is in attack range, switch to attack state
        if (distanceToPlayer < attackRange)
        {
            currentState = AIState.Attacking;
        }
        // If player is out of chase range, switch back to patrol state
        else if (distanceToPlayer > patrolRange)
        {
            currentState = AIState.Patrolling;
            GoToNextWaypoint();
        }
    }

    private void Attack()
    {
        // Stop moving to attack
        agent.isStopped = true;

        // If player moves out of attack range, go back to chasing
        if (!HasLineOfSightToPlayer(attackRange + 1f)) // Use a slightly larger range to prevent rapid state switching
        {
            currentState = AIState.Chasing;
            return;
        }

        // Rotate to face the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Shoot at the player if cooldown is ready
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
                // Could not find a valid point, use the enemy's start position as a fallback
                patrolPoints[i] = transform.position;
            }
        }
    }

    /// <summary>
    /// Checks if the enemy has a clear line of sight to the player within a given range.
    /// </summary>
    private bool HasLineOfSightToPlayer(float range)
    {
        if (playerTransform == null) return false;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > range) return false;

        // Cast a ray from the enemy towards the player. If it hits an object on a sight-blocking layer first, there is no line of sight.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, sightBlockingLayers);

        // If hit.collider is null, it means the ray didn't hit any sight-blocking objects before reaching the player's position.
        return hit.collider == null;
    }
}