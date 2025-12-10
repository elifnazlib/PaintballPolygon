using UnityEngine;

// This script is used to swing the camera slightly in the menu
public class CameraSwing : MonoBehaviour
{
    [SerializeField] private float positionAmplitude = 0.05f;   // How much it moves
    [SerializeField] private float rotationAmplitude = 1f;      // How much it rotates
    [SerializeField] private float frequency = 0.2f;            // How fast it rotates

    private Vector3 _startPos;
    private Quaternion _startRot;

    private void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
    }

    private void Update()
    {
        // It works even if you set Time.timeScale = 0 in the menu
        float t = Time.unscaledTime * frequency;

        // Small oscillation in position
        float offsetX = Mathf.Sin(t) * positionAmplitude;
        float offsetY = Mathf.Cos(t * 0.7f) * positionAmplitude;

        transform.position = _startPos + new Vector3(offsetX, offsetY, 0f);
        
        // Small oscillation in rotation
        float rotY = Mathf.Sin(t * 1.3f) * rotationAmplitude;
        float rotX = Mathf.Cos(t * 0.9f) * rotationAmplitude;
        
        transform.rotation = _startRot * Quaternion.Euler(rotX, rotY, 0f);
    }
}