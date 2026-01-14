using System;
using UnityEngine;
using UnityEngine.Pool;


public class PooledGun : MonoBehaviour
{

    [SerializeField] private PooledBullets bulletPrefab;
    [SerializeField] private Transform spawnHere;
    [SerializeField] private float fireRate;

    private float fireTime;

    private IObjectPool<PooledBullets> pool;

    void Awake()
    {
        pool = new ObjectPool<PooledBullets>(
           createFunc: CreateBullet, 
           actionOnGet: OnGetBullet,//called whenev pool hands our an obj
           actionOnRelease: OnReleaseBullet,
           actionOnDestroy: OnDestroyItemBullet, //called if over maxsize
           collectionCheck: true,   // helps catch double release mistakes
           defaultCapacity: 10,
           maxSize: 50
       );
    }


    private PooledBullets CreateBullet()
    {
        PooledBullets bullet = Instantiate(bulletPrefab);
        bullet.SetPool(pool);
        return bullet;
    }
    private void OnGetBullet(PooledBullets bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(PooledBullets bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyItemBullet(PooledBullets bullet)
    {
        Destroy(bullet.gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C) && Time.time >= fireTime)
        {
            fireTime = Time.time + 1 / fireRate;
            PooledBullets _bullets = pool.Get();
            _bullets.transform.SetLocalPositionAndRotation(spawnHere.position, spawnHere.rotation);
            _bullets.Fire(spawnHere.forward);
        }
    }
}
