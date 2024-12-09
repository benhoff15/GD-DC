using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 5f; // Range at which the enemy detects the player

    private Transform player;
    private bool isCombatTriggered = false;

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
            Debug.LogError("Player not found! Ensure the Player GameObject has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player == null || isCombatTriggered) return;

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
        isCombatTriggered = true;

        TurnBasedCombatSystem combatSystem = FindObjectOfType<TurnBasedCombatSystem>();
        if (combatSystem == null)
        {
            Debug.LogError("TurnBasedCombatSystem not found in the scene!");
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogError("Player object not found! Ensure the Player GameObject has the 'Player' tag.");
            return;
        }

        Debug.Log("Player detected! Starting combat...");
        combatSystem.StartCombat(gameObject, playerObject);

        // Optional: Disable this script during combat to prevent re-triggering
        this.enabled = false;
    }
}