using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : PooledMonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Vector2 offset;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Transform healthTarget;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(healthTarget.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 localPoint);

        localPoint += offset;

        rectTransform.anchoredPosition = localPoint;
    }
    public void HealthbarTarget(Transform healthTarget)
    {
        this.healthTarget = healthTarget;
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthBarFill.enabled = true;
        backgroundImage.enabled = true;
        healthBarFill.fillAmount = (float)currentHealth / (float)maxHealth;
    }

    internal void SetCanvas(Canvas canvas)
    {
        this.canvas = canvas;
        transform.parent = canvas.transform;
        healthBarFill.enabled = false;
        backgroundImage.enabled = false;
    }

    public void CallReturnToPool()
    {
        ReturnToPool(0);
    }
}
