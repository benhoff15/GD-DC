using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public TurnBasedCombatSystem combatSystem; // Reference to the turn-based combat system
    public float detectionRange = 10f; // Range at which the enemy detects the player

    private Transform player;

    void Start()
    {
        // Find the player object by its tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Ensure the player GameObject has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null) return;

        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within detection range, start combat
        if (distanceToPlayer < detectionRange)
        {
            StartCombat();
        }
    }

    private void StartCombat()
    {
        if (combatSystem == null)
        {
            Debug.LogError("CombatSystem not assigned! Assign it in the Inspector.");
            return;
        }

        Debug.Log("Player detected! Starting combat...");
        combatSystem.StartCombat(gameObject, player.gameObject); // Pass the enemy and player to the combat system

        // Optional: Disable this script during combat to prevent re-triggering
        this.enabled = false;
    }
}
