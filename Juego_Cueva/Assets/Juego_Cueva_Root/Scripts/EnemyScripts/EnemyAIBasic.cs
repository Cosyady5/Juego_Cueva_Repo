using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Libreria para usar clases de NavMesh.

public class EnemyAIBasic : MonoBehaviour
{

    private Animator anim;

    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent; // Componente que permite al objeto tener IA.
    [SerializeField] Transform target; // Transform del objeto a perseguir.
    [SerializeField] LayerMask targetLayer; // Capa de detección del target.
    [SerializeField] LayerMask groundLayer; // Capa de detección del suelo.

    [Header("Patroling Stats")]
    public Vector3 walkPoint; // Dirección a la que se movera la IA si no se detecta al target.
    [SerializeField] float walkPointRange; // Distancia máxima de dirección a generar.
    [SerializeField] bool walkPointSet; // Determina si la IA ha llegado al objetivo

    [Header("Attack Configuration")]
    public float timeBetweenAttacks; // Tiempo de espera entre ataques.
    private bool alredyAttacked; // Determina si ya ha atacado.
    [SerializeField] Collider attackCollider;
    public int damage;

    [Header("Speed Settings")]
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float chaseSpeed = 5f;

    [Header("States & Detection")]
    [SerializeField] float sightRange; // Distancia de detección del target de la IA.
    [SerializeField] float attackRange; // Distancia de ataque.
    [SerializeField] bool targetInSightRange; // Determina si el target esta a distancia de detección.
    [SerializeField] bool targetInAttacktRange; // Determina si el target esta a distancia de ataque.

    enum EnemyState { Idle, Patrol, Chase, Attack }
    EnemyState currentState;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player").transform;
        attackCollider.enabled = false;
    }

    private void Update()
    {
        HandleAnimations();
        EnemyStateUpdater();
    }

    void HandleAnimations()
    {
        float speed = agent.velocity.magnitude / Mathf.Max(agent.speed, 0.01f);

        anim.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        switch (currentState)
        {
            case EnemyState.Patrol:
                anim.SetFloat("Velocity", 0.4f);
                break;
            case EnemyState.Chase:
                anim.SetFloat("Velocity", 1.0f);
                break;
            default:
                anim.SetFloat("Velocity", 0f);
                break;
        }

    }
    void EnemyStateUpdater()
    {
        // Revisar si el target esta en los rangos de detección y/o ataque:
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInAttacktRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        // Cambios dinámicos de estado de la IA:
        // Orden de prioridades: ataque > persecución > patrulla.
        if (!targetInSightRange && !targetInAttacktRange)
        {
            Patroling();
        }
        if (targetInSightRange && !targetInAttacktRange)
        {
            ChaseTarget();
        }
        if (targetInSightRange && targetInAttacktRange)
        {
            AttackTarget();
        }
    }

    void Patroling()
    {
        agent.speed = patrolSpeed;
        currentState = EnemyState.Patrol;

        if (!walkPointSet)
        {
            // Genera un punto de caminado nuevo:
            SearchWalkPoint();
        }
        else
        {
            // Mueve el agente al nuevo punto de caminado:
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        // Generación de nuevo punto de caminado:
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        // Fijación nuevo punto de caminado:
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Comprobación de si el nuevo punto de caminado es válido:
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    void ChaseTarget()
    {
        agent.speed = chaseSpeed;
        currentState = EnemyState.Chase;
        //agent.SetDestination(target.position);

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > agent.stoppingDistance)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.SetDestination(transform.position); // Para detenerse si ya está cerca
        }
    }

    void AttackTarget()
    {
        /*agent.ResetPath();
        agent.velocity = Vector3.zero;
        // Antes de atacar...:
        //agent.SetDestination(transform.position); // Evita que se mueva.
        anim.SetTrigger("isAttacking");
        transform.LookAt(target);
        */
        if (!alredyAttacked)
        {
            currentState = EnemyState.Attack;

            agent.ResetPath();
            agent.velocity = Vector3.zero;
            //agent.isStopped = true;

            anim.SetTrigger("isAttacking");
            transform.LookAt(target);

            alredyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alredyAttacked = false;
        //if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh) { agent.isStopped = false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackCollider.enabled) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            attackCollider.enabled = false;
        }
    }

    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }
    // Función para que los Gizmos de detección (perseguir/ataque) se dibujen en la escena al seleccionar el objeto.
    private void OnDrawGizmosSelected()
    {
        // Dibuja el rango de ataque:
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Dibuja el rango de persecución:
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
