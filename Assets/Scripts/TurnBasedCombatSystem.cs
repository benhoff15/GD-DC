using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnBasedCombatSystem : MonoBehaviour
{
    public enum TurnState { PlayerTurn, EnemyTurn, CombatEnd }
    private TurnState currentTurn;

    private GameObject player;
    private GameObject enemy;


    private void Start()
    {
        // Ensure combat only starts in the combat scene
        if (SceneManager.GetActiveScene().name != GameManager.instance.TurnBasedBattleScene)
        {
            Debug.Log("Not in the combat scene. Combat system will not initialize.");
            enabled = false;
            return;
        }

        Debug.Log("Combat scene loaded. Initializing combat...");

        // Find Player and Enemy
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (player != null && enemy != null)
        {
            StartCombat(enemy, player);
        }
        else
        {
            Debug.LogError("Player or Enemy not found in the Combat Scene!");
        }
    }

    public void StartCombat(GameObject enemyObj, GameObject playerObj)
    {
        player = playerObj;
        enemy = enemyObj;

        if (player == null)
        {
            Debug.LogError("Player is null at the start of combat.");
        }
        if (enemy == null)
        {
            Debug.LogError("Enemy is null at the start of combat.");
        }

        Debug.Log("Combat started!");
        currentTurn = TurnState.PlayerTurn;
        StartCoroutine(CombatLoop());
    }

    private IEnumerator CombatLoop()
    {
        while (currentTurn != TurnState.CombatEnd)
        {
            if (player == null || enemy == null)
            {
                Debug.LogWarning("Player or Enemy is null. Ending combat.");
                currentTurn = TurnState.CombatEnd;
                break;
            }

            switch (currentTurn)
            {
                case TurnState.PlayerTurn:
                    Debug.Log("Player's Turn");
                    yield return PlayerTurn();
                    if (enemy == null || enemy.GetComponent<Health>().currentHealth <= 0)
                    {
                        EndCombat();
                        yield break;
                    }
                    currentTurn = TurnState.EnemyTurn;
                    break;

                case TurnState.EnemyTurn:
                    Debug.Log("Enemy's Turn");
                    yield return EnemyTurn();
                    if (player.GetComponent<Health>().currentHealth <= 0)
                    {
                        EndCombat();
                        yield break;
                    }
                    currentTurn = TurnState.PlayerTurn;
                    break;
            }
        }
    }
    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player's Turn. Waiting for action...");
        bool actionSelected = false;

        // Wait for player input
        while (!actionSelected)
        {
            if (Input.GetKeyDown(KeyCode.A)) // Example: Attack
            {
                Debug.Log("Player chose to attack!");
                DealDamage(enemy, 10);
                actionSelected = true;
            }
            else if (Input.GetKeyDown(KeyCode.D)) // Example: Defend
            {
                Debug.Log("Player chose to defend!");
                // Implement defend logic here
                actionSelected = true;
            }

            yield return null; // Wait until the next frame
        }

        Debug.Log("Player turn finished.");
        yield break;
    }

    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy's Turn.");
        yield return new WaitForSeconds(1); // Simulate thinking time

        // Enemy attacks the player
        DealDamage(player, 5);

        Debug.Log("Enemy turn finished.");
        yield break;
    }

    private void DealDamage(GameObject target, int damage)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null or has been destroyed. Skipping damage.");
            return;
        }

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
        else
        {
            Debug.LogError("Target does not have a Health component.");
        }
    }

    public void OnTargetDeath(GameObject target)
    {
        if (target == enemy)
        {
            Debug.Log("Enemy defeated, ending combat.");
            EndCombat();
        }
        else if (target == player)
        {
            Debug.Log("Player defeated, game over.");
            EndCombat(); 
        }
    }
    private void EndCombat()
    {
        Debug.Log("Combat has ended.");
        GameManager.instance.isEnemyDefeated = true; // Mark the enemy as defeated

        // Safely destroy the enemy
        if (enemy != null)
        {
            Destroy(enemy);
            Debug.Log("Enemy destroyed in the combat scene.");
        }

        // Transition back to the main scene
        GameManager.instance.ExitCombat(player);
    }
}


