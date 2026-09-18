using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public enum EnemyState2
{
    Patrolling,
    Following,
    Attacking
}

public class EnemyController2 : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float losePlayerTime = 3f;
    [SerializeField] private float attackRange = 2f;

    [SerializeField] private float attackDuration = 2f;
    [SerializeField] private float lookAtSpeed = 10f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private EnemyState2 _state = EnemyState2.Patrolling;
    private float _timeSinceLostPlayer;
    private bool _isAttacking;

    [Header("Speed")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float followSpeed = 3f;

    [Header("Acceleration")]
    [SerializeField] private float patrolAccel = 10f;
    [SerializeField] private float followAccel = 10f;

    private float patrolRange = 10f;



    private bool gameOverStarted;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        Patrol();
    }
    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch (_state)
        {
            case EnemyState2.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                {
                    _state = EnemyState2.Following;
                }

                break;

            case EnemyState2.Following:
                FollowPlayer();
                if (distanceToPlayer <= attackRange)
                {
                    _state = EnemyState2.Attacking;
                    StartAttack();
                }
                if (!CanSeePlayer())
                {
                    _timeSinceLostPlayer += Time.deltaTime;
                    if (_timeSinceLostPlayer >= losePlayerTime)
                    {
                        _state = EnemyState2.Patrolling;
                        Patrol();
                    }
                }
                else
                {
                    _timeSinceLostPlayer = 0f;
                }

                break;

            case EnemyState2.Attacking:
                Attack();
                if (!_isAttacking && distanceToPlayer > attackRange)
                {
                    _state = EnemyState2.Following;
                    _agent.isStopped = false;
                }

                break;
        }

        UpdateAnimations();
    }

    private void StartAttack()
    {
        if (gameOverStarted)
            return;

        gameOverStarted = true;

        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;

        _isAttacking = true;

        _animator.SetTrigger("Attacker");

        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        // Disable player movement/look
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        Look playerLook = player.GetComponent<Look>();

        if (playerMove != null)
            playerMove.enabled = false;

        if (playerLook != null)
            playerLook.updatingRotation = false;

        float timer = 0f;

        while (timer < attackDuration)
        {
            timer += Time.deltaTime;

            // Make player look at enemy
            Vector3 direction = transform.position - player.position;
            direction.y = 2f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                player.rotation = Quaternion.Slerp(player.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
            }

            yield return null;
        }

        // Make absolutely sure the player is facing the enemy
        Vector3 finalDirection = transform.position - player.position;
        finalDirection.y = 2f;

        if (finalDirection != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(finalDirection);
        }

        // Load Game Over scene
        SceneManager.LoadScene("GameOver");
    }
    private void Attack()
    {
        _agent.isStopped = true;

        var direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnAttackAnimationEnd()
    {
        _isAttacking = false;
    }

    private void FollowPlayer()
    {
        _agent.speed = followSpeed;
        _agent.acceleration = followAccel;

        _agent.SetDestination(player.position);
    }

    private void Patrol()
    {
        _agent.speed = patrolSpeed;
        _agent.acceleration = patrolAccel;

        if (_agent.remainingDistance <= _agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(transform.position, patrolRange, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                _agent.SetDestination(point);
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void UpdateAnimations()
    {
        var Walk = _agent.velocity.sqrMagnitude > 0.01f;
        _animator.SetBool("Walk", Walk);
    }

    private bool CanSeePlayer()
    {
        return IsFacingPlayer() && HasClearPathToPlayer();
    }

    private bool IsFacingPlayer()
    {
        var dirToPlayer = (player.position - transform.position).normalized;
        var angle = Vector3.Angle(transform.forward, dirToPlayer);
        return angle <= viewAngle / 2f;
    }

    private bool HasClearPathToPlayer()
    {
        var dirToPlayer = player.position - transform.position;
        if (Physics.Raycast(transform.position, dirToPlayer.normalized, out RaycastHit hit, dirToPlayer.magnitude))
        {
            return hit.transform == player;
        }

        return true;
    }
}