using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public Transform[] patrolPoints;
    public int currentPatrolPoint;

    public NavMeshAgent agent;
   
    public Animator anim;

    public enum AIState
    {
        isIdle, isPatrolling, isChasing, isAttacking
    };

    public AIState currentState;

    public float waitAtPoint = 2f;
    private float waitCounter;

    public float chaseRange;
    public float attackRange = 1f;

    public float timeBetweenAttacks = 2f;
    private float attackCounter;

    // Start is called before the first frame update


    void Start()
    {
        waitCounter = waitAtPoint;
    }

    // Update is called once per frame
    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);


        switch (currentState)
        {
            case AIState.isIdle:
                Idle(distanceToPlayer);
                break;

            case AIState.isPatrolling:

                Patroll(distanceToPlayer);
                break;

            case AIState.isChasing:

                Chase(distanceToPlayer);
                break;
            case AIState.isAttacking:

                Attack(distanceToPlayer);
                break;

        }   
    }



    protected virtual void Idle(float distanceToPlayer)
    {
        anim.SetBool("IsMoving", false);
        if (waitCounter > 0)
        {
            waitCounter -= Time.deltaTime;
        }
        else
        {
            currentState = AIState.isPatrolling;
            agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        }

        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.isChasing;
            anim.SetBool("IsMoving", true);
        }
    }

    protected virtual void Patroll(float distanceToPlayer)
    {
        if (agent.remainingDistance <= .2f)
        {
            currentPatrolPoint++;
            if (currentPatrolPoint >= patrolPoints.Length)
            {
                currentPatrolPoint = 0;
            }
            //agent.SetDestination(patrolPoints[currentPatrolPoint].position);
            currentState = AIState.isIdle;
            waitCounter = waitAtPoint;
        }
        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.isChasing;
        }

        anim.SetBool("IsMoving", true);
    }

    protected virtual void Chase(float distanceToPlayer)
    {
        agent.SetDestination(PlayerController.instance.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = AIState.isAttacking;
            anim.SetTrigger("Attack");
            anim.SetBool("IsMoving", false);
            agent.velocity = Vector3.zero;
            agent.isStopped = true;

            attackCounter = timeBetweenAttacks;
        }
        if (distanceToPlayer > chaseRange)
        {
            currentState = AIState.isIdle;
            waitCounter = waitAtPoint;
            agent.velocity = Vector3.zero;
            agent.SetDestination(transform.position);
        }
    }

    protected virtual void Attack(float distanceToPlayer)
    {
        transform.LookAt(PlayerController.instance.transform, Vector3.up);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        attackCounter -= Time.deltaTime;
        if (attackCounter <= 0)
        {
            if (distanceToPlayer < attackRange)
            {
                anim.SetTrigger("Attack");
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                currentState = AIState.isIdle;
                waitCounter = waitAtPoint;

                agent.isStopped = false;
            }

        }
    }
}
