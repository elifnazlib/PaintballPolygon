using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

// This script is used to control the raycast of the weapon.
public class Weapon : MonoBehaviour
{
    private GameManager gameManager; // GameManager instance to update the score
    [SerializeField] private int forceMultiplier = 10; // Multiplier for the force applied to the inner circles
    [SerializeField] private float minDurationForDisappear = 5f, maxDurationForDisappear = 10f; // Max and min durations for disappear of target boards after getting shot
    [SerializeField] private Bullet bullet; // Bullet instance to shoot
    [SerializeField] private GameObject bulletHolePrefab; // Bullet hole prefab to instantiate when the ray hits something
    [SerializeField] private Animator paintballGunAnimator; // Paintball gun animator reference for recoil effect
    [SerializeField] private AudioClip shootSound; // Sound to play when the player shoots
    [SerializeField] private GameObject muzzle; // Muzzle location for shoot sound and effects                         
    // [SerializeField] private GameObject splashSprite;
    [SerializeField] private ParticleSystem splashParticleSystem;
    [SerializeField] private int comboCount = 0;
    [SerializeField] private float comboTimer = 0f, comboResetTime = 5f;
    [SerializeField] private bool comboActive = false;

    [SerializeField] private GameObject gameOverbulletHolePrefab; // Bullet hole prefab to show overall accuracy at the end of the game
    [SerializeField] private GameObject gameOverTargetBoard; // Target board to show overall accuracy at the end of the game
    [SerializeField] private float y_offset = 0.27f;

    private void Start()
    {
        gameManager = (GameManager)FindFirstObjectByType(typeof(GameManager)); // Finding the GameManager instance (for better performance)
    }

    void Update()
    {
        if (comboActive)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }

        if (Input.GetButtonDown("Fire1"))
        { // If the player presses the left mouse button
            Recoil();
            Shoot(); // Shoot the ray
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);  // Debugging
    }

    // This method shoots a ray from the camera to the forward direction
    private void Shoot()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // Creating a ray from the camera to the forward direction
        bullet.ShootBullet();
        splashParticleSystem.Play();

        RaycastHit hitData; // Storing the hit data

        if (Physics.Raycast(ray, out hitData)) // If the ray hits something
        {
            GameObject hitGameObject = hitData.collider.gameObject; // TODO: Do we need arrays to detect all the objects?
            
            // GameObject parentOfHitGameObject = hitGameObject.transform.parent.gameObject; // Getting the parent of the hit object

            // TargetBoard parentTargetBoard = parentOfHitGameObject.GetComponent<TargetBoard>();

            // GameObject splashPS = Instantiate(splashParticleSystemPrefab, hitData.point + hitData.normal * 0.01f, Quaternion.LookRotation(-hitData.normal));

            if (hitGameObject.CompareTag("TargetBoard"))
            {
                /*
                GameObject splash = Instantiate(splashSprite, hitData.point + hitData.normal * 0.01f, Quaternion.LookRotation(hitData.normal), hitGameObject.transform);
                splash.transform.localScale *= 0.1f;
                */

                GameObject parentOfHitGameObject = hitGameObject.transform.parent.gameObject; // Getting the parent of the hit object

                TargetBoard parentTargetBoard = parentOfHitGameObject.GetComponent<TargetBoard>();

                GameObject bulletHole = Instantiate(bulletHolePrefab, hitData.point + hitData.normal * 0.001f, Quaternion.LookRotation(hitData.normal), hitGameObject.transform); // Instantiating the bullet hole prefab at the hit point with the normal rotation

                if (parentTargetBoard.CanUpdateScore)
                {
                    Vector3 localHitPoint = parentTargetBoard.transform.InverseTransformPoint(hitData.point);
                    localHitPoint.y = y_offset;
                    Vector3 gameOverHitPoint = gameOverTargetBoard.transform.TransformPoint(localHitPoint);
                    
                    Instantiate(gameOverbulletHolePrefab,
                        gameOverHitPoint, 
                        Quaternion.LookRotation(hitData.normal), 
                        gameOverTargetBoard.transform);
                    
                    // If the ray hits Target Board
                    comboCount++;
                    comboTimer = comboResetTime;
                    comboActive = true;
                    
                    // TODO: Get hit point on current target board and instantiate the bullet hole prefab on the game over target board

                    List<GameObject> listOfSiblings = new List<GameObject>(); // List of siblings of the parent of the hit object
                    foreach (Transform sibling in parentOfHitGameObject.transform) // Getting the siblings of the parent of the hit object
                    {
                        listOfSiblings.Add(sibling.gameObject); // Adding the sibling to the list
                    }

                    float randomDurationForDisappear = UnityEngine.Random.Range(minDurationForDisappear, maxDurationForDisappear); // Random duration for creation (Used UnityEngine.Random.Range() to generate random floats)

                    // Stopping scoring
                    gameManager.UpdateScore(hitGameObject.name, listOfSiblings[5]); // Updating the score according to the hit object
                    parentTargetBoard.CanUpdateScore = false; // Preventing the multiple score updates for the same target board
                    parentTargetBoard.IsShot = true; // Preventing the movements on the ground

                    foreach (GameObject inner in listOfSiblings)
                    {
                        if (inner == listOfSiblings[5]) continue; // Skipping the canvas object

                        // Fall down or tear apart
                        inner.GetComponent<MeshCollider>().convex = true; // Making the inner circles convex
                        Rigidbody rb = inner.AddComponent(typeof(Rigidbody)) as Rigidbody; // Adding a rigidbody to the inner circles

                        rb.useGravity = true; // Applying gravity to the inner circles
                        rb.AddForce(-Vector3.forward * forceMultiplier, ForceMode.Impulse); // Applying an impulse force to the inner circles
                        //## Fall down or tear apart

                        // Destroy(inner, randomDurationForDisappear); // Destroys the inners after waiting "randomDurationForDisappear"
                    }
                    Destroy(parentOfHitGameObject, randomDurationForDisappear); // Destroys the target board game object after waiting "randomDurationForDisappear"
                }
                else
                {
                    // If the target board has already been shot
                    ResetCombo();
                }
            }
            else
            {
                /*
                GameObject splash = Instantiate(splashSprite, hitData.point + hitData.normal * 0.01f, Quaternion.LookRotation(hitData.normal));
                splash.transform.localScale *= 0.1f;
                */

                GameObject bulletHole = Instantiate(bulletHolePrefab, hitData.point + hitData.normal * 0.001f, Quaternion.LookRotation(hitData.normal)); // Instantiating the bullet hole prefab at the hit point with the normal rotation
                // Moving the bullet hole slightly forward to avoid z-fighting

                // If the ray hits something other than Target Board
                ResetCombo();
            }

            bullet.DestroyYourself();
        }
        else
        {
            // If the ray hits nothing
            ResetCombo();
        }
    }

    private void Recoil()
    {
        paintballGunAnimator.SetTrigger("RecoilTrigger"); // Triggering the recoil animation of the paintball gun
        AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position); // Playing the shoot sound at the camera position
        //AudioSource.PlayClipAtPoint(shootSound, muzzle.transform.position); // Playing the shoot sound at the muzzle position
    }

    // This method resets combo when the player shoots something different from target board or shoots nothing
    void ResetCombo()
    {
        if (comboActive && comboCount > 0)
        {
            Debug.Log($"Combo ended at x{comboCount}");
        }
        comboCount = 0;
        comboActive = false;
        comboTimer = 0;
    }
}
