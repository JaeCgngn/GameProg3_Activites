using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{

    [Header ("Attack Settings")]
    public float attackRange = 3f;
    public float normalDamage = 10f;
    public float normalCooldown = 0.5f;
    float lastAttackTime;
    private Enemy enemy;
    GameObject targetEnemy;
    NavMeshAgent agent;
    // [Header ("Skill Settings")]
    // public float skillDamage = 30f;
    // public float skillCooldown = 5f;

    // private NavMeshAgent agent;
    // private Enemy enemy;
    // private float lastAttackTime = 0f;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        GameObject target = FindEnemyInRange(); // find enemy in range
        AttackRangeDetection(); // handle movement toward target
    }

    GameObject FindEnemyInRange() // find the closest enemy within attack range
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // get all enemies in the scene

        GameObject closestEnemy = null; // to store the closest enemy
        float closestDistance = Mathf.Infinity; // start with infinite distance

        foreach (GameObject enemy in enemies) // iterate through all enemies
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position); // distance to enemy

            if (distance <= attackRange && distance < closestDistance) // within range and closer than previous
            {
                closestDistance = distance; // update closest distance
                closestEnemy = enemy; // update closest enemy

            }
        }

        return closestEnemy;
    }

    void Attack()
    {
        Debug.Log("Attacking enemy");
        // later: animations, damage, cooldown, etc.
    }

    void AttackRangeDetection() // handle movement toward target enemy
    {
        if (targetEnemy == null) return;

        float distance = Vector3.Distance(transform.position, targetEnemy.transform.position); // distance to target

        if (distance <= attackRange) 
        {
            if (!agent.isStopped)
                agent.isStopped = true; // stop movement

            Debug.Log("Enemy in Range - Attacking!"); // attack when in range
            Attack();
        }
        else
        {
            if (agent.isStopped) 
                agent.isStopped = false; // resume movement

            if (agent.destination != targetEnemy.transform.position) // update destination if changed
                agent.SetDestination(targetEnemy.transform.position); // move toward the enemy
        }
    }

    public void SetTarget(GameObject enemy)
    {
         targetEnemy = enemy; // set the target enemy

        if (agent != null) // ensure agent is not null
        {
            agent.isStopped = false; // ensure agent is moving
            agent.SetDestination(targetEnemy.transform.position); // move toward the enemy
            Debug.Log("Moving toward enemy");
        }

        Debug.Log("Enemy clicked");
    }

    public void ClearTarget()
    {
        targetEnemy = null; // clear the target
        if (agent != null) // ensure agent is not stopped
            agent.isStopped = false; // resume movement
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
