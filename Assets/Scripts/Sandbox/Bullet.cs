using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool shootBullet = false;
    [SerializeField] private GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    private Rigidbody bulletRb; // Rigidbody of the bullet
    private GameObject instantiatedBullet; // The instantiated bullet object
    private Vector3 targetPosition; // The target position of the bullet
    private Vector3 bulletDirection; // The vector3 of the bullet
    [SerializeField] private int bulletVelocity = 50; // The velocity of the bullet
    [SerializeField] private int distanceThreshold = 10; // TODO: Find a sweet spot for this value
    
    public void ShootBullet()
    {
        targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceThreshold;
        bulletDirection = (targetPosition - transform.position).normalized;
        shootBullet = true; // Set the shootBullet flag to true
        instantiatedBullet = Instantiate(bulletPrefab, transform.position, transform.rotation); // Instantiate the bullet prefab at the current position and rotation
        bulletRb = instantiatedBullet.GetComponent<Rigidbody>(); // Get the Rigidbody component of the bullet prefab
    }

    private void FixedUpdate()
    {
        if (shootBullet)
        {
            bulletRb.AddForce(bulletDirection * bulletVelocity, ForceMode.Impulse); // Add force to the bullet in the forward direction
            shootBullet = false;
        }
    }
    
    // TODO: Set target position to the hit point if the ray hits something
    // TODO: Destroy yourself after hitting anything
    // TODO: Destroy yourself if you do not hit target board after some time
}
