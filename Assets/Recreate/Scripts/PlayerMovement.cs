using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
        NavMeshAgent agent;
        Camera cam;
        public float detectionRange = 5f;
        GameObject currentTarget;

        PlayerAttack attack;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        attack = GetComponent<PlayerAttack>();

    }

    void Update()
    {
        PlayerMove();
        //DetectEnemies();
    }

    void PlayerMove()
    {
        //VERSION 1 - CLICK TO MOVE
        // if (Input.GetMouseButtonDown(0)) 
        // { 
        //     Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
        //     RaycastHit hit; 

        //     if (Physics.Raycast(ray, out hit)) 
        //     { 
        //         agent.SetDestination(hit.point);
        //     } 
        // }

        //VERSION 2 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // CLICKED ENEMY
                if (hit.collider.CompareTag("Enemy"))
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    attack.SetTarget(enemy.gameObject);
                    return;
                }

                // CLICKED GROUND
                attack.ClearTarget();
                agent.SetDestination(hit.point);
            }
        }

    }

    // void DetectEnemies()
    // {

    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 

    //     foreach (GameObject enemy in enemies)
    //     {
    //         float distance = Vector3.Distance(transform.position, enemy.transform.position); 

    //         if (distance <= detectionRange) 
    //         {
    //             Debug.Log("Enemy detected"); 
    //         }
    //     }
    // }

    //  void HandleAttack()
    // {
    //     if (currentTarget == null) return;

    //     if (attack.IsInRange(currentTarget))
    //     {
    //         agent.ResetPath();
    //         attack.Attack(currentTarget);
    //     }
    // }

    // void Attack(GameObject enemy)
    // {
    //     Debug.Log("Attacking the enemy!");
    //     //agent.SetDestination(transform.position);
    //     agent.ResetPath();
    // }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
