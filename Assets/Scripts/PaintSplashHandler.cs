using UnityEngine;
using System.Collections.Generic;

public class PaintSplashHandler : MonoBehaviour
{
    public GameObject splashMarkPrefab; // Yüzeye yapışacak görsel
    public float splashLifetime = 5f;

    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void OnParticleCollision(GameObject other)
    {
        int numEvents = ParticlePhysicsExtensions.GetCollisionEvents(GetComponent<ParticleSystem>(), other, collisionEvents);

        for (int i = 0; i < numEvents; i++)
        {
            Vector3 hitPoint = collisionEvents[i].intersection;
            Vector3 hitNormal = collisionEvents[i].normal;

            Quaternion rot = Quaternion.LookRotation(hitNormal);
            Vector3 offsetPos = hitPoint + hitNormal * 0.001f;

            GameObject splash = Instantiate(splashMarkPrefab, offsetPos, rot);
            splash.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
            splash.transform.SetParent(other.transform);

            Destroy(splash, splashLifetime);
        }
    }
}
