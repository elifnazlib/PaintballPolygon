using System.Collections.Generic;
using UnityEngine;

// This script is used to control the raycast of the weapon.
public class Weapon : MonoBehaviour
{
    private GameManager gameManager; // GameManager instance to update the score
    [SerializeField] private int forceMultiplier = 10; // Multiplier for the force applied to the inner circles
    [SerializeField] private float minDurationForDisappear = 5f, maxDurationForDisappear = 10f; // Max and min durations for disappear of target boards after getting shot
    
    private void Start() { 
        gameManager = (GameManager)FindFirstObjectByType(typeof(GameManager)); // Finding the GameManager instance (for better performance)
    }
    
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) { // If the player presses the left mouse button
            Shoot(); // Shoot the ray
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);  // Debugging
    }

    // This method shoots a ray from the camera to the forward direction
    private void Shoot() {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); // Creating a ray from the camera to the forward direction
        
        RaycastHit hitData; // Storing the hit data
        
        if (Physics.Raycast(ray, out hitData)) // If the ray hits something
        {
            GameObject hitGameObject = hitData.collider.gameObject; // TODO: Do we need arrays to detect all the objects?
            GameObject parentOfHitGameObject = hitGameObject.transform.parent.gameObject; // Getting the parent of the hit object
            
            TargetBoard parentTargetBoard = parentOfHitGameObject.GetComponent<TargetBoard>();
            
            if (hitGameObject.CompareTag("TargetBoard") && parentTargetBoard.CanUpdateScore)
            {
                List<GameObject> listOfSiblings = new List<GameObject>(); // List of siblings of the parent of the hit object
                foreach (Transform sibling in parentOfHitGameObject.transform) // Getting the siblings of the parent of the hit object
                {
                    listOfSiblings.Add(sibling.gameObject); // Adding the sibling to the list
                }

                float randomDurationForDisappear= UnityEngine.Random.Range(minDurationForDisappear, maxDurationForDisappear); // Random duration for creation (Used UnityEngine.Random.Range() to generate random floats)

                // Stopping scoring
                gameManager.UpdateScore(hitGameObject.name); // Updating the score according to the hit object
                parentTargetBoard.CanUpdateScore = false; // Preventing the multiple score updates for the same target board
                parentTargetBoard.IsShot = true; // Preventing the movements on the ground

                // DEBUGGING
                // foreach (GameObject inner in listOfSiblings)
                // {
                //     inner.GetComponent<MeshRenderer>().materials[0].color = Color.red; // Changing the color of the inner circles
                // }
                //## DEBUGGING
                
                foreach (GameObject inner in listOfSiblings)
                {
                    // Fall down or tear apart
                    inner.GetComponent<MeshCollider>().convex = true; // Making the inner circles convex
                    Rigidbody rb = inner.AddComponent(typeof(Rigidbody)) as Rigidbody; // Adding a rigidbody to the inner circles

                    rb.useGravity = true; // Applying gravity to the inner circles
                    rb.AddForce(Vector3.forward * forceMultiplier, ForceMode.Impulse); // Applying an impulse force to the inner circles
                    //## Fall down or tear apart

                    Destroy(inner, randomDurationForDisappear); // Destroys the inners after waiting "randomDurationForDisappear"
                }
            }
            
        }
    }
}
