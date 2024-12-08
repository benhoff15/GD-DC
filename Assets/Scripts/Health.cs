using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Slider healthBar;

    void Start()
    {
        // Restore health from GameManager or default to maxHealth
        currentHealth = GameManager.instance != null ? GameManager.instance.playerCurrentHealth : maxHealth;

        // Debug for initialization
        Debug.Log($"{name} initialized with {currentHealth}/{maxHealth} health.");
    }

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth; // Set the slider value to match current health
        }
    }

    public void TakeDamage(int damage)
    {
        
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (healthBar != null)
            {
                healthBar.value = currentHealth; // Update the slider dynamically
            }

        // Update GameManager health
        if (GameManager.instance != null)
        {
            GameManager.instance.playerCurrentHealth = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} has died!");
        Destroy(gameObject);
    }
}
