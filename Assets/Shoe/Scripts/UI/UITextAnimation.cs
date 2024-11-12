using System.Collections;
using TMPro;
using UnityEngine;

public class UITextAnimation : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float enlargementDuration = 2.0f;
    public float fadeDuration = 2.0f;
    public float maxFontSize = 60f;
    private float initialFontSize;
    private Color initialColor;

    private void Awake()
    {
        // Cache the initial font size and color
        initialFontSize = textMeshPro.fontSize;
        initialColor = textMeshPro.color;

        // Ensure the text starts fully opaque
        Color color = textMeshPro.color;
        color.a = 1f;
        textMeshPro.color = color;

        // Ensure the text starts at the initial size
        textMeshPro.fontSize = initialFontSize;
    }

    private void OnEnable()
    {
        StartCoroutine(EnlargeAndFade());
    }

    private IEnumerator EnlargeAndFade()
    {
        float timer = 0f;

        // Enlarge the font size
        while (timer < enlargementDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / enlargementDuration;
            textMeshPro.fontSize = Mathf.Lerp(initialFontSize, maxFontSize, progress);
            yield return null;
        }

        // Ensure the font size is exactly the max size at the end of the enlargement
        textMeshPro.fontSize = maxFontSize;

        // Reset the timer for the fade effect
        timer = 0f;

        // Fade the text
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            Color color = textMeshPro.color;
            color.a = Mathf.Lerp(1f, 0f, progress);
            textMeshPro.color = color;
            yield return null;
        }

        // Ensure the text is fully transparent at the end of the fade
        Color finalColor = textMeshPro.color;
        finalColor.a = 0f;
        textMeshPro.color = finalColor;

        // Optionally, disable the GameObject after the effect
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Reset the text properties for reuse
        textMeshPro.fontSize = initialFontSize;
        textMeshPro.color = initialColor;
    }
}
