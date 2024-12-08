using UnityEngine;
using UnityEngine.UI;

public class CombatSceneInitializer : MonoBehaviour
{
    public GameObject player; // Assign the player object if it’s not persistent

    private void Start()
    {
        // Ensure the player exists
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            InitializeHealthBar(player);
        }
        else
        {
            Debug.LogError("Player not found in combat scene!");
        }
    }

    private void InitializeHealthBar(GameObject player)
    {
        Health playerHealth = player.GetComponent<Health>();
        GameObject healthBarObject = GameObject.Find("PlayerHealthBar"); // Ensure this matches your object's name
        if (healthBarObject != null)
        {
            Slider healthBar = healthBarObject.GetComponent<Slider>();
            if (healthBar != null)
            {
                healthBar.maxValue = playerHealth.maxHealth;
                healthBar.value = playerHealth.currentHealth;

                playerHealth.SetHealthBar(healthBar); // Link the slider to the player's health
            }
        }
        else
        {
            Debug.LogError("PlayerHealthBar not found in the combat scene.");
        }
    }
}
