using UnityEngine;

public class WeaponRayCast : MonoBehaviour
{
    private GameManager gameManager;
    private void Start() {
        gameManager = (GameManager)FindFirstObjectByType(typeof(GameManager));
    }
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red);
    }

    private void Shoot() {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData))
        {
            // Debug.Log(hitData.collider.gameObject.name);
            gameManager.UpdateScore(hitData.collider.gameObject.name);
        }
    }
}
