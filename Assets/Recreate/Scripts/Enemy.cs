using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{

    public float EnemyHealth = 100f;
    public float initialMoveSpeed = 2f;

    [Header("Enemy Movement")]
    private float moveRange = 3f;
    private float currentMoveSpeed;
    private Vector3 startPos;
    private int direction = 1; 
    private Coroutine moveCoroutine;

    [Header("Materials")]
    public Material normalMaterial;
    public Material slowMaterial;
    private Renderer enemyRenderer;



    void Start()
    {
        startPos = transform.position;
        currentMoveSpeed = initialMoveSpeed;
        //moveCoroutine = StartCoroutine(Move());
        enemyRenderer = GetComponentInChildren<Renderer>();
        enemyRenderer.material = normalMaterial;
    }

    void Update()
    {
        LoopMovement();
    }

    void LoopMovement()
    {
        transform.Translate(Vector3.right * direction * currentMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) >= moveRange)
        {
            direction *= -1;
            startPos = transform.position;
        }
    }


    public void TakeDamage(float damageAmount)
    {
        EnemyHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. HP: " + EnemyHealth);
        
        if (EnemyHealth <= 0f)
        {
            Die();
        }
    }


    public void ApplySlow(float slowPercent, float duration)
    {
        Debug.Log("Apply Slow Active");
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine); 

        moveCoroutine = StartCoroutine(SlowEffect(slowPercent, duration));
    }

    IEnumerator SlowEffect(float slowPercent, float duration)
    {
        currentMoveSpeed = initialMoveSpeed * (1f - slowPercent);
        SetSlowedVisual(true);

        yield return new WaitForSeconds(duration);

        currentMoveSpeed = initialMoveSpeed;
        SetSlowedVisual(false);
        moveCoroutine = null;
    }

    void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
    }

    void SetSlowedVisual(bool slowed)
    {
        if (enemyRenderer == null) return;

        enemyRenderer.material = slowed ? slowMaterial : normalMaterial;
    }
}
