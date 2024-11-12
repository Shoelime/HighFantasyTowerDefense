using System.Collections;
using UnityEngine;

public class MaterialAlphaFade : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public float fadeDuration = 1f;
    Color color;

    void OnEnable()
    {
        color = meshRenderer.material.color;
        StartCoroutine(FadeToTransparent());
    }

    IEnumerator FadeToTransparent()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the new alpha value based on the current time
            float alpha = Mathf.Lerp(0.5f, 0f, elapsedTime / fadeDuration);

            meshRenderer.material.color = new Color(color.r, color.g, color.b, alpha);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}