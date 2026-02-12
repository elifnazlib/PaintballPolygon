using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private ParticleSystem splashParticleSystem;

    private ObjectPool pool;
    
    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverBulletHolePrefab; // Bullet hole prefab to show overall accuracy at the end of the game
    [SerializeField] private GameObject gameOverTargetBoard; // Target board to show overall accuracy at the end of the game
    [SerializeField] private float yOffset = 0.27f;
    public int totalShotsFired = 0;
    public int totalHits = 0;
    
    [Header("Combo Settings")]
    [SerializeField] private int comboCount = 0;
    [SerializeField] private int maxComboCount = 5;
    [SerializeField] private float comboTimer = 0f;
    [SerializeField] private bool comboActive = false;
    [SerializeField] private float comboIncreaseRate = 0.5f;
    [SerializeField] private float comboDecayRate = 0.1f;
    [SerializeField] private Slider comboSlider;
    [SerializeField] private TextMeshProUGUI comboText;
    private Color comboColor;

    private void Start()
    {
        gameManager = (GameManager)FindFirstObjectByType(typeof(GameManager)); // Finding the GameManager instance (for better performance)
        pool = FindAnyObjectByType<ObjectPool>();
        // Reset cursor after scene restart
        Cursor.lockState = CursorLockMode.Locked; // Locking the cursor to the center of the screen
        Cursor.visible = false; // Hiding the cursor
    }

    void Update()
    {
        if (comboActive)
        {
            if (comboTimer > maxComboCount)
            {
                comboTimer = maxComboCount;
            }
            
            comboCount = (int) (comboTimer + 1);  // Ekranda x şeklinde gösterim için
            comboText.text = $"X{comboCount}";
            comboSlider.value = comboTimer + 1 - comboCount;
            comboTimer -= Time.deltaTime * comboDecayRate;
            
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
        
        // If the player presses the left mouse button
        if (Input.GetButtonDown("Fire1") && !gameManager.isGameOver)
        { 
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
        // splashParticleSystem.Play();

        RaycastHit hitData; // Storing the hit data

        totalShotsFired++;

        if (Physics.Raycast(ray, out hitData)) // If the ray hits something
        {
            GameObject hitGameObject = hitData.collider.gameObject; // TODO: Do we need arrays to detect all the objects?

            if (hitGameObject.CompareTag("TargetBoard"))
            {
                GameObject parentOfHitGameObject = hitGameObject.transform.parent.gameObject; // Getting the parent of the hit object
                TargetBoard parentTargetBoard = parentOfHitGameObject.GetComponent<TargetBoard>();
                
                // GameObject bulletHole = Instantiate(bulletHolePrefab, hitData.point + hitData.normal * 0.001f, Quaternion.LookRotation(hitData.normal), hitGameObject.transform); // Instantiating the bullet hole prefab at the hit point with the normal rotation
                GameObject bulletHole = pool.GetBulletHoleFromPool();
                bulletHole.transform.position = hitData.point + hitData.normal * 0.001f;
                bulletHole.transform.rotation = Quaternion.LookRotation(hitData.normal);
                bulletHole.transform.SetParent(hitGameObject.transform);
                StartCoroutine(pool.DeactivateBulletHole(bulletHole));
                
                if (parentTargetBoard.CanUpdateScore)
                {
                    Vector3 localHitPoint = parentTargetBoard.transform.InverseTransformPoint(hitData.point);
                    localHitPoint.y = yOffset;
                    Vector3 gameOverHitPoint = gameOverTargetBoard.transform.TransformPoint(localHitPoint);
                    
                    Instantiate(gameOverBulletHolePrefab,
                        gameOverHitPoint, 
                        Quaternion.LookRotation(hitData.normal), 
                        gameOverTargetBoard.transform);
                    
                    
                    // If the ray hits Target Board
                    totalHits++;
                    
                    comboTimer += comboIncreaseRate;
                    comboActive = true;
                    Debug.Log("Combo x" + comboCount);
                    
                    // TODO: Get hit point on current target board and instantiate the bullet hole prefab on the game over target board

                    List<GameObject> listOfSiblings = new List<GameObject>(); // List of siblings of the parent of the hit object
                    foreach (Transform sibling in parentOfHitGameObject.transform) // Getting the siblings of the parent of the hit object
                    {
                        listOfSiblings.Add(sibling.gameObject); // Adding the sibling to the list
                    }

                    float randomDurationForDisappear = UnityEngine.Random.Range(minDurationForDisappear, maxDurationForDisappear); // Random duration for creation (Used UnityEngine.Random.Range() to generate random floats)

                    if (comboCount > maxComboCount)
                    {
                        comboCount = maxComboCount;
                    }
                    
                    // Stopping scoring
                    gameManager.UpdateScore(hitGameObject.name, listOfSiblings[5], Mathf.Max(1, comboCount)); // Updating the score according to the hit object
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
                    }

                    StartCoroutine(SetParentOfBulletHole(hitGameObject, randomDurationForDisappear - 0.1f));
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
                // Moving the bullet hole slightly forward to avoid z-fighting
               
                // GameObject bulletHole = Instantiate(bulletHolePrefab, hitData.point + hitData.normal * 0.001f, Quaternion.LookRotation(hitData.normal)); // Instantiating the bullet hole prefab at the hit point with the normal rotation
                GameObject bulletHole = pool.GetBulletHoleFromPool();
                bulletHole.transform.position = hitData.point + hitData.normal * 0.001f;
                bulletHole.transform.rotation = Quaternion.LookRotation(hitData.normal);
                StartCoroutine(pool.DeactivateBulletHole(bulletHole));
                
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
        if (comboActive)
        {
            Debug.Log($"Combo ended at X{comboCount}");
        }
        comboCount = 0;
        comboActive = false;
        comboTimer = 0f;
        
        comboSlider.value = 0f;
        comboText.text = "X1";
    }
    
    // This method sets the parent of the bullet hole to null
    // for not getting destroyed with the target board
    IEnumerator SetParentOfBulletHole(GameObject bulletHole, float time)
    {
        yield return new WaitForSeconds(time);
        bulletHole.transform.GetChild(0).SetParent(null);
    }
}
