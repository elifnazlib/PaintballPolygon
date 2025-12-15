using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    public float mouseSensitivity;  // To retrieve mouse sensitivity set by player
    public Image crosshairImage;    // To retrieve crosshair settings set by player

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        mouseSensitivity = 1f;
    }
}
