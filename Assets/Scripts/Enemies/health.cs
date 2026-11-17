using UnityEngine;
using UnityEngine.UI;

public class HealthTarget : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Reference")]
    public Image healthFillImage;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} Destroyed!");
            Destroy(gameObject); 
        }
    }

    void UpdateHealthBar()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
