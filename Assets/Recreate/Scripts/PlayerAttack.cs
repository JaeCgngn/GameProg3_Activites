using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{

    [Header ("Attack Settings")]
    public float attackRange = 3f;
    public float normalCooldown = 1f;
    public int normalDamage = 3;
    private float lastAttackTime;


    [Header ("Objects & Components")]
    private Enemy enemy;
    GameObject targetEnemy;
    public GameObject weaponPrefab;
    NavMeshAgent agent;
    private Enemy enemyHealth;
    public Transform firePoint;
    private LineRenderer line;
    private Camera cam;


    [Header ("Skill Settings")]
    public GameObject skillProjectilePrefab;
    public float skillRange = 8f;
    public float skillCooldown = 10f;
    public float skillRadius = 1.5f;
    public float skillDamageMultiplier = 1.8f;
    public LayerMask groundLayer;
    private bool isAimingSkill;
    private float lastSkillTime;



    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        cam = Camera.main;

        line.enabled = false;
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

        if (Time.time < lastAttackTime + normalCooldown)
            return;

        lastAttackTime = Time.time;
        SpawnWeapon();
        Debug.Log("Normal Attack!");

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
            line.enabled = true;
            Debug.Log("Aiming Skill Attack - Click to Cast");
        }

        if (isAimingSkill)
        {
            UpdateAimLine();
        }


        if(Input.GetKeyUp(KeyCode.E))
        {
            
           if (Time.time >= lastSkillTime + skillCooldown) // check cooldown
            {
                SkillAttack();
                
                lastSkillTime = Time.time;
            }
            else
            {
                Debug.Log("Skill on Cooldown");
            }
            
            isAimingSkill = false;
            line.enabled = false;
        }
    }

    void SkillAttack()
    {
        Debug.Log("Skill Attack Active");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
 
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 hitPoint = hit.point; // get hit point
            hitPoint.y = transform.position.y; // align y position

            Vector3 direction = hitPoint - transform.position;
            float distance = direction.magnitude;


            Vector3 skillPoint = transform.position + direction.normalized * Mathf.Min(distance, skillRange); // limit to skill range

            Debug.Log("Skill Spawned");

            SpawnSkillProjectile();
        }
    }

    void SpawnSkillProjectile()
    {
        Debug.Log("Spawn Skill Porjectile Called");
        Vector3 direction = (line.GetPosition(1) - firePoint.position).normalized;

        GameObject projObj = Instantiate(
            skillProjectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        SkillProjectile proj = projObj.GetComponent<SkillProjectile>();

        int skillDamage = Mathf.RoundToInt(normalDamage * 1.8f);

        proj.Initialize(direction, skillDamage);

        Debug.Log("Skill projectile thrown");
    }

    void UpdateAimLine()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 start = firePoint.position;
            Vector3 dir = (hit.point - start).normalized;
            Vector3 end = start + dir * skillRange;

            line.SetPosition(0, start);
            line.SetPosition(1, end);
        }
    }

    void DealSkillDamage(Vector3 skillPoint)
    {
        Collider[] hits = Physics.OverlapSphere(skillPoint, skillRadius);

    int skillDamage = Mathf.RoundToInt(
        normalDamage + (normalDamage * skillDamageMultiplier)
    );

    foreach (Collider col in hits)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy health = col.GetComponent<Enemy>();
            if (health != null)
            {
                health.TakeDamage(skillDamage);
                Debug.Log("Skill hit " + col.name + " for " + skillDamage);
            }
        }
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
