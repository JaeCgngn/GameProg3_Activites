using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SkillProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 15f;
    public float maxLifetime = 3f;

    [Header("Hit")]
    public GameObject hitEffectPrefab;


    public float hitRadius = 0.4f;
    
    private int damage;
    private Vector3 direction;

    [Header ("Skill Effects")]
    public float slowPercent = 0.6f;
    public float slowDuration = 2f;



    public void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    public void Initialize(Vector3 dir, int dmg) // call this to set target point and damage
    {
        direction = dir.normalized;
        damage = dmg;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        
    }


    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Enemy")) return;

        Enemy enemy =other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Skill projectile hit " + other.name + " for " + damage + " damage");
            enemy.ApplySlow(slowPercent, slowDuration);
            Debug.Log("Applied slow to " + other.name + " for " + (slowPercent * 100) + "% for " + slowDuration + " seconds");
        }

        SpawnHitEffect();
        Destroy(gameObject);

    }


    void SpawnHitEffect()
    {
        if (hitEffectPrefab == null) return;

        GameObject fx = Instantiate(
            hitEffectPrefab,
            transform.position,
            Quaternion.identity
        );

        Destroy(fx, 2f); 
    }

}
