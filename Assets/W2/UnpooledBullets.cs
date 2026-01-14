using UnityEngine;

public class UnpooledBullets : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float lifetime;

    [SerializeField] float timer;

    void Start()
    {
        
    }
    public void Fire(Vector3 _direction)
    {
        timer = 0;
        transform.position = _direction;
    }


    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        timer = Time.deltaTime;
        if(timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
