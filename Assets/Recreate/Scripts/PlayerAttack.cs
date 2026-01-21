using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 3f;
    public float normalDamage = 10f;
    public float normalCooldown = 0.5f;


    private Transform camTransform;
    private Enemy target;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ClickAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
}
