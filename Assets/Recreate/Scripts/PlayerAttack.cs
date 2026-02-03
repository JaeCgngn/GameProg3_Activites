using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{

    [Header ("Attack Settings")]
    public float attackRange = 3f;
    public float normalDamage = 10f;
    public float normalCooldown = 0.5f;

    [Header ("Skill Settings")]
    public float skillDamage = 30f;
    public float skillCooldown = 5f;

    private NavMeshAgent agent;
    private Enemy enemy;
    private float lastAttackTime = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        
    }

   
}
