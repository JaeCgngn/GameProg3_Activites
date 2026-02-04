using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 3;
    private Enemy target;


    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    void Start()
    {
        
    }

    void Update()
    {   
        //Debug.Log("Target: " + target);
        Vector3 dir = (target.transform.position - transform.position).normalized; // direction to target
        transform.position += dir * speed * Time.deltaTime; // move toward target
        HitEnemy();

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    void HitEnemy()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position); // check distance to target
        if (distance < 0.3f) 
        {
            target.TakeDamage(damage); // apply damage to target
            Debug.Log("Projectile dealt " + damage + " damage to " + target.name);
            Destroy(gameObject); 
        }
    }
}
