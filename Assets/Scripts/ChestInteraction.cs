using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestInteraction : MonoBehaviour
{
    private CombatUIManager combatUIManager;

    void Start()
    {
        // Find the CombatUIManager in the scene
        combatUIManager = FindObjectOfType<CombatUIManager>();
        if (combatUIManager == null)
        {
            Debug.LogError("CombatUIManager not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player interacts with the chest
        if (other.CompareTag("Player"))
        {
            if (combatUIManager != null)
            {
                // Log the interaction
                combatUIManager.AddToCombatLog("You found a chest! Restarting the game...");
            }
            Debug.Log("Player interacted with the chest. Restarting the game...");
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
