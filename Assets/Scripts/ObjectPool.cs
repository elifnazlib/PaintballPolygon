using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletHolePrefab;
    private Queue<GameObject> bulletPool = new();
    private Queue<GameObject> bulletHolePool = new();
    
    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(bulletHolePrefab);
            obj.SetActive(false);
            bulletHolePool.Enqueue(obj);
        }
    }

    public GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            GameObject obj = bulletPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return Instantiate(bulletPrefab);
    }

    public void ReturnBulletToPool(GameObject obj)
    {
        obj.SetActive(false);
        bulletPool.Enqueue(obj);
    }
    
    public IEnumerator DeactivateBullet(GameObject obj, Rigidbody rb)
    {
        yield return new WaitForSeconds(3f);
        if (!obj.activeSelf) yield break;
        
        rb.velocity = Vector3.zero; // Resetting velocity before returning to pool
        ReturnBulletToPool(obj); // Return to Pool
    }
    
    
    public GameObject GetBulletHoleFromPool()
    {
        if (bulletHolePool.Count > 0)
        {
            GameObject obj = bulletHolePool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return Instantiate(bulletHolePrefab);
    }
    
    public void ReturnBulletHoleToPool(GameObject obj)
    {
        obj.SetActive(false);
        bulletHolePool.Enqueue(obj);
    }
    
    public IEnumerator DeactivateBulletHole(GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        if (!obj.activeSelf) yield break;
        
        ReturnBulletHoleToPool(obj); // Return to Pool
    }
    
}
