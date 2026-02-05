using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 3;
    public float hitRadius = 0.3f;
    private Enemy target;

    public GameObject hitEffectPrefab;


    public void SetTarget(Enemy enemy)
    {
        target = enemy;
    }

    void Start()
    {
        
    }

    void Update()
    {   

        Vector3 dir = (target.transform.position - transform.position).normalized; // direction to target
        transform.position += dir * speed * Time.deltaTime; // move toward target

        HitEnemy();

        // if (target == null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
    }

    void HitEnemy()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position); // check distance to target
        if (distance < 0.3f) // hit radius
        {
            target.TakeDamage(damage); // apply damage to target
            SpawnHitEffect();
            Debug.Log("Projectile dealt " + damage + " damage to " + target.name);
            Destroy(gameObject); 
        }
    }

    void SpawnHitEffect()
    {
        if (hitEffectPrefab == null) return;
        Debug.Log("Spawning hit effect at " + transform.position);
        GameObject fx = Instantiate(
            hitEffectPrefab,
            transform.position,
            Quaternion.identity
        );

        Destroy(fx, 1.5f); 
    }
    
}
