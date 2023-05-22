using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Move")]
    NavMeshAgent agent;

    [Header("Necessary Items")]
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    [HideInInspector] public CapsuleCollider col;
    [HideInInspector] public Animator animator;

    [Header("Look")]
    float turnSpeed = 0.1f;
    Quaternion rotGoal;
    Vector3 lookDirection;
    Vector3 direction;
    bool canRotate;

    [Header("Basic Attack")]
    public float basicAttackRange;
    public float coolDownTime = 2f;
    [HideInInspector] public bool playerInBasicAttackRange;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        col = gameObject.GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        canRotate = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerInBasicAttackRange = Physics.CheckSphere(transform.position, basicAttackRange, whatIsPlayer);

        if (!playerInBasicAttackRange)
        {
            agent.speed = 6;
            Chase();
        }

        if (playerInBasicAttackRange)
        {
            agent.speed = 0;
        }

        if (canRotate == true)
        {
            lookDirection = new Vector3(player.position.x, transform.position.y, player.position.z);
            direction = (lookDirection - transform.position).normalized;
            rotGoal = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, basicAttackRange);
    }
}
