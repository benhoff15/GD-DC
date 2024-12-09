using UnityEngine;
using UnityEngine.UI;

public class CombatSceneInitializer : MonoBehaviour
{
   //ignore this script, was used for a deleted idea
    public GameObject player; 

    private void Start()
    {
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
        GameObject healthBarObject = GameObject.Find("PlayerHealthBar"); 
        if (healthBarObject != null)
        {
            Slider healthBar = healthBarObject.GetComponent<Slider>();
            if (healthBar != null)
            {
                healthBar.maxValue = playerHealth.maxHealth;
                healthBar.value = playerHealth.currentHealth;

            }
        }
        else
        {
            Debug.LogError("PlayerHealthBar not found in the combat scene.");
        }
    }
}
