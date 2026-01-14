using UnityEngine;


public class UnpooledGun : MonoBehaviour
{
    [SerializeField] private UnpooledBullets bulletPrefab;
    [SerializeField] private Transform spawnHere;
    [SerializeField] private float fireRate;

    private float fireTime;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && Time.time >= fireTime)
        {
            fireTime = Time.time + 1 / fireRate;
            UnpooledBullets _bullets = Instantiate(bulletPrefab, spawnHere.position, spawnHere.rotation);
            _bullets.Fire(spawnHere.forward);
        }
    }
}
