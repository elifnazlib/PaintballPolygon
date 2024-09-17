using UnityEngine;

// This script is used to control the raycast of the weapon.
public class WeaponRayCast : MonoBehaviour
{
    private GameManager gameManager; // GameManager instance to update the score
    
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
            // Debug.Log(hitData.collider.gameObject.name);
            gameManager.UpdateScore(hitData.collider.gameObject.name); // Updating the score according to the hit object
        }
    }
}
