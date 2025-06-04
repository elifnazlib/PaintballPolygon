using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float floatSpeed = 20f;
    public float fadeDuration = 1.5f;

    private TextMeshProUGUI tmp;
    private CanvasGroup canvasGroup;
    private Vector3 moveDirection = Vector3.up;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(string message, Color color)
    {
        tmp.text = message;
        tmp.color = color;
        StartCoroutine(FadeAndMove());
    }

    System.Collections.IEnumerator FadeAndMove()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;

            // Yukarı doğru hareket
            transform.position = startPos + moveDirection * floatSpeed * t;

            // Fade-out
            canvasGroup.alpha = 1f - t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}