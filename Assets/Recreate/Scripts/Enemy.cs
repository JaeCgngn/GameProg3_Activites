using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float EnemyHealth = 100f;

    public void TakeDamage(float damageAmount)
    {
        EnemyHealth -= damageAmount;
        if (EnemyHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        
        Destroy(gameObject);
    }
}
