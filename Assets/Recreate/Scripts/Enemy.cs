using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float EnemyHealth = 100f;

    public void TakeDamage(float damageAmount)
    {
        EnemyHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. HP: " + EnemyHealth);
        
        if (EnemyHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }
}
