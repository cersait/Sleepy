using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public enum EnemyState
{
    Patrolling,
    Following,
    Attacking
}

public class EnemyController : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("Walk");

    [SerializeField] private Transform player;
    [SerializeField] private Transform[] patrolPoints;

    [SerializeField] private float patrolWaitTime = 0.5f;
    [SerializeField] private float stopAtDistance = 0.5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private float losePlayerTime = 3f;
    [SerializeField] private float attackRange = 2f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private EnemyState _state = EnemyState.Patrolling;
    private int _currentPatrolIndex;
    private bool _isWaiting;
    private float _timeSinceLostPlayer;
    private bool _isAttacking;

    [SerializeField] private float attackDuration = 2f;
    [SerializeField] private float lookAtSpeed = 10f;

    private bool gameOverStarted;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        GoToNextPatrolPoint();
    }
    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch(_state)
        {
            case EnemyState.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRange && CanSeePlayer())
                {
                    _state = EnemyState.Following;
                }

                break;

            case EnemyState.Following:
                FollowPlayer();
                if (distanceToPlayer <= attackRange)
                {
                    _state = EnemyState.Attacking;
                    StartAttack();
                }
                if (!CanSeePlayer())
                {
                    _timeSinceLostPlayer += Time.deltaTime;
                    if (_timeSinceLostPlayer >= losePlayerTime)
                    {
                        _state = EnemyState.Patrolling;
                        GoToClosestPatrolPoint();
                    }
                }
                else
                {
                    _timeSinceLostPlayer = 0f;
                }

                break;

            case EnemyState.Attacking:
                Attack();
                if (!_isAttacking && distanceToPlayer > attackRange)
                {
                    _state = EnemyState.Following;
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

                player.rotation = Quaternion.Slerp(
                    player.rotation,
                    targetRotation,
                    lookAtSpeed * Time.deltaTime
                );
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
        _agent.SetDestination(player.position);
    }
    private void Patrol()
    {
        if (_isWaiting) return;
        if (!_agent.pathPending && _agent.remainingDistance <= stopAtDistance)
        {
            StartCoroutine(WaitAtPatrolPoint());
        }
    }

    private IEnumerator WaitAtPatrolPoint()
    {
        _isWaiting = true;
        _agent.isStopped = true;

        yield return new WaitForSeconds(patrolWaitTime);

        _agent.isStopped = false;
        GoToNextPatrolPoint();
        _isWaiting = false;
    }

    private void GoToClosestPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        var closestIndex = 0;
        var closestDistance = float.MaxValue;

        for (var i = 0; i < patrolPoints.Length; i++)
        {
            var distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        _currentPatrolIndex = closestIndex;
        _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        {
            _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
            _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
        }
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
