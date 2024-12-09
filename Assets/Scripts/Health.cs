using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Damage multipliers
    public float manaMagicMultiplier = 1f;
    public float swiftStrikeMultiplier = 1f;

    void Start()
    {
        currentHealth = maxHealth;

        // Set multipliers based on the GameObject's name
        if (name.Contains("Wizard"))
        {
            manaMagicMultiplier = 0.5f;  // Wizards take less damage from Mana Magic
            swiftStrikeMultiplier = 1.5f;  // Wizards take more damage from Swift Strike
        }
        else
        {
            // Default multipliers for normal enemies
            manaMagicMultiplier = 1f;
            swiftStrikeMultiplier = 1f;
        }
    }

    public void TakeDamage(int damage, string attackType)
    {
        float finalDamage = damage;

        // Apply damage multiplier based on attack type
        if (attackType == "ManaMagic")
        {
            finalDamage *= manaMagicMultiplier;
        }
        else if (attackType == "SwiftStrike")
        {
            finalDamage *= swiftStrikeMultiplier;
        }

        currentHealth -= Mathf.RoundToInt(finalDamage);
        Debug.Log($"{name} took {Mathf.RoundToInt(finalDamage)} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // currentHealth doesn't go over maxHealth
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"{name} healed by {amount}. Current health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{name} has died!");
        TurnBasedCombatSystem combatSystem = FindObjectOfType<TurnBasedCombatSystem>();
        if (combatSystem != null && gameObject.CompareTag("Enemy"))
        {
            combatSystem.OnEnemyDeath(name);
        }
        Destroy(gameObject);
    }
}
