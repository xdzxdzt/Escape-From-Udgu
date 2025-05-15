using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints;
    public float idleTime = 2f;
    public float walkSpeed = 2f; 
    public float chaseSpeed = 4f; 
    public float sightDistance = 10f;
    public AudioClip movementSound;
    public AudioSource audioSource;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private float idleTimer = 0f;
    private Transform player;

    private float nextMovementSoundTime = 0f; 
    private float nextGurglingSoundTime = 10f;

    private enum EnemyState { Idle, Walk, Chase }
    private EnemyState currentState = EnemyState.Idle;

    private bool isChasingAnimation = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Настройка поворота камеры
        agent.angularSpeed = 100f; 
        agent.acceleration = 35f; 
        agent.stoppingDistance = 0.5f; 

        SetDestinationToWaypoint();
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                idleTimer += Time.deltaTime;
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsChasing", false); 

                if (idleTimer >= idleTime)
                {
                    NextWaypoint();
                }

                CheckForPlayerDetection();
                break;

            case EnemyState.Walk:
                idleTimer = 0f;
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsChasing", false);

               

                PlayMovementSound(walkSpeed);

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = EnemyState.Idle;
                }

                CheckForPlayerDetection();
                break;

            case EnemyState.Chase:
                idleTimer = 0f;
                agent.speed = chaseSpeed; 
                agent.SetDestination(player.position);
                isChasingAnimation = true; 
                animator.SetBool("IsChasing", true);

              

                PlayMovementSound(chaseSpeed);

                if (Vector3.Distance(transform.position, player.position) > sightDistance)
                {
                    currentState = EnemyState.Walk;
                    agent.speed = walkSpeed;
                }
                break;
        }

        HandleRotation();
    }

    private void HandleRotation()
    {

        if (agent.velocity.sqrMagnitude > 0.1f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
        }
    }

    private void CheckForPlayerDetection()
    {
        RaycastHit hit;
        Vector3 playerDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                currentState = EnemyState.Chase;
                Debug.Log("Player detected!");
            }
        }
    }

    private void PlayMovementSound(float speed)
    {

        float movementDelay = 0f;


        if (currentState == EnemyState.Walk)
        {
            movementDelay = 0.65f; 
        }
        else if (currentState == EnemyState.Chase)
        {
            movementDelay = 0.5f; 
        }

        if (Time.time >= nextMovementSoundTime)
        {
            audioSource.PlayOneShot(movementSound); 
            nextMovementSoundTime = Time.time + movementDelay;
        }
    }


    private void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        SetDestinationToWaypoint();
    }

    private void SetDestinationToWaypoint()
    {
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentState = EnemyState.Walk;
        agent.speed = walkSpeed; 
        animator.enabled = true;
    }
}
