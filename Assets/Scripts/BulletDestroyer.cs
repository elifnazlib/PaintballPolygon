using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    private float bulletTimeAlive = 3f; // Time after which the bullet will be destroyed if it doesn't hit anything
    private float bulletTimer = 0f; // Timer to track the bullet's lifetime

    private void Update()
    {
        bulletTimer += Time.deltaTime; // Increment the bullet timer
        if (bulletTimer >= bulletTimeAlive)
        {
            Debug.Log("Bullet has been alive for too long, destroying it.");
            Destroy(gameObject); // Destroy the bullet if it has been alive for too long
        }
    }
}