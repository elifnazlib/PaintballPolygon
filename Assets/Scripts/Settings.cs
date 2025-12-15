using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    public float mouseSensitivity;  // To retrieve mouse sensitivity set by player

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
