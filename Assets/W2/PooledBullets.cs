using UnityEngine;
using UnityEngine.Pool;

public class PooledBullets : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float lifetime;

    public float timer;

    private IObjectPool<PooledBullets> pool;

    public void SetPool(IObjectPool<PooledBullets> owningpool)
    {
        pool = owningpool;
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
        if (timer >= lifetime)
        {
            pool.Release(this);
        }
    }
}
