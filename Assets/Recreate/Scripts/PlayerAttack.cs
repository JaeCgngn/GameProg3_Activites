using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{

    [Header ("Attack Settings")]
    public float attackRange = 3f;
    //public int normalDamage = 3;
    public float normalCooldown = 1f;
    private Enemy enemy;
    GameObject targetEnemy;
    public GameObject weaponPrefab;
    NavMeshAgent agent;
    private Enemy enemyHealth;
    private float lastAttackTime;


    [Header ("Skill Settings")]
    public float skillRange = 8f;
    public float skillCooldown = 10f;
    public float skillRadius = 1.5f;
    public float skillDamageMultiplier = 0.8f;
    private bool isAimingSkill;
    private float lastSkillTime;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        GameObject target = FindEnemyInRange(); // find enemy in range
        AttackRangeDetection(); // handle movement toward target
        HandleSkillAttack(); // handle skill attack
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

    void NormalAttack()
    {
        //Debug.Log("Attacking enemy");

        if (Time.time < lastAttackTime + normalCooldown)
            return;

        lastAttackTime = Time.time;
        SpawnWeapon();
        Debug.Log("Normal Attack!");

        // if (enemyHealth != null)
        // {
        //     //enemyHealth.TakeDamage(normalDamage);
        //     Debug.Log("Dealt " + normalDamage + " damage");
        // }
    }

    void SpawnWeapon()
    {
        GameObject weapon = Instantiate(weaponPrefab, transform.position + transform.forward, Quaternion.identity); // spawn weapon in front of player
        
        Projectile proj = weapon.GetComponent<Projectile>(); // get Projectile component
        proj.SetTarget(enemyHealth);
    }

    void HandleSkillAttack()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            isAimingSkill = true;
            Debug.Log("Aiming Skill Attack - Click to Cast");
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            
           if (Time.time >= lastSkillTime + skillCooldown)
            {
                SkillAttack();
                //Debug.Log("Casting Skill Attack");
                lastSkillTime = Time.time;
            }
            else
            {
                Debug.Log("Skill on Cooldown");
            }
            isAimingSkill = false;
        }
    }

    void SkillAttack()
    {
        //Debug.Log("Casting Skill Attack");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = (hit.point - transform.position).normalized;
            Vector3 skillPoint = transform.position + direction * skillRange;

            Debug.Log("Skill cast!");

            // SpawnSkillEffect(skillPoint);
            // DealSkillDamage(skillPoint);
        }
    }

    void AttackRangeDetection() // handle movement toward target enemy
    {
        if (targetEnemy == null) return;

        float distance = Vector3.Distance(transform.position, targetEnemy.transform.position); // distance to target

        if (distance <= attackRange) 
        {
            if (!agent.isStopped)
                agent.isStopped = true; // stop movement

            //Debug.Log("Enemy in Range - Attacking!"); // attack when in range
            NormalAttack();
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
         enemyHealth = enemy.GetComponent<Enemy>();

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
