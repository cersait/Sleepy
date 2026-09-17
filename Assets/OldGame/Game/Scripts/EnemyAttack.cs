using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemyattack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 2f; //Om vi är 2 från spelaren ska enemy attakera
    public GameObject theEnemy;
    private bool isAttacking = false; //Börjar på false
    private NavMeshAgent stalkerAgent;
    private Animator enemyAnimator;
    public int damageAmount = 5; //Ge 5 skada

    void Start()
    {
        stalkerAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = theEnemy.GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttacking) //Attackera om vi är i attackrange
        {
            isAttacking = true;
            Attack();
            StartCoroutine(AttackCoroutine());
        }
        else if (distanceToPlayer > attackRange && isAttacking) //Attackera inte om vi inte är i attackrange
        {
            StopAttack();
        }

        if (!isAttacking) //Följ spelaren och visa walk anim om vi inte är nära spelaren
        {
            stalkerAgent.SetDestination(player.position); 
            enemyAnimator.Play("Walk");
        }
    }

    IEnumerator AttackCoroutine()
    {
        while (isAttacking)
        {
            yield return new WaitForSeconds(3f); //Attack anim är seg, så vi måste fördröja spelningen 
            if (isAttacking)
            {
                GlobalHealth.currentHealth -= damageAmount; //Vänta med att ge damage tills vi har blivit attackerade
            }
        }
    }

    void Attack()
    {
        isAttacking = true;
        stalkerAgent.isStopped = true; // Stanna enemy från att röra sig
        enemyAnimator.Play("Attack"); //Spela attack anim
    }

    void StopAttack()
    {
        isAttacking = false;
        stalkerAgent.isStopped = false; //Starta enemys movement igen efter att attack inte är true
        StopCoroutine(AttackCoroutine());
    }
}
