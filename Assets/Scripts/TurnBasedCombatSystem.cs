using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnBasedCombatSystem : MonoBehaviour
{
    public CombatUIManager combatUIManager;

    private GameObject player;
    private GameObject enemy;

    public enum TurnState { PlayerTurn, EnemyTurn, CombatEnd }
    private TurnState currentTurn;

    private int manaCooldown = 0; // Cooldown for Mana Magic attack

    public void StartCombat(GameObject enemyObj, GameObject playerObj)
    {
        player = playerObj;
        enemy = enemyObj;

        Debug.Log("Combat started!");
        combatUIManager.ClearCombatLog();
        combatUIManager.AddToCombatLog($"{enemy.name} has appeared!");

        Health playerHealth = player.GetComponent<Health>();
        Health enemyHealth = enemy.GetComponent<Health>();

        combatUIManager.UpdateHealthBar(combatUIManager.PlayerHealthBar, playerHealth.currentHealth, playerHealth.maxHealth);

        Image enemyHealthBar = FindEnemyHealthBar(enemy);
        if (enemyHealthBar != null)
        {
            combatUIManager.SetEnemyHealthBar(enemyHealthBar);
        }

        combatUIManager.StartClearingCombatLog(7f);
        combatUIManager.ShowCombatUI();

        currentTurn = TurnState.PlayerTurn;
        StartCoroutine(CombatLoop());
    }

    private IEnumerator CombatLoop()
    {
        while (currentTurn != TurnState.CombatEnd)
        {
            switch (currentTurn)
            {
                case TurnState.PlayerTurn:
                    Debug.Log("Player's Turn");
                    combatUIManager.SetButtonsInteractable(true); 
                    yield return new WaitUntil(() => currentTurn == TurnState.EnemyTurn);
                    combatUIManager.SetButtonsInteractable(false); 
                    break;

                case TurnState.EnemyTurn:
                    Debug.Log("Enemy's Turn");
                    yield return EnemyTurn();

                    if (CheckCombatEnd()) yield break; 

                    currentTurn = TurnState.PlayerTurn;
                    break;
            }

            if (manaCooldown > 0)
            {
                manaCooldown--;
                Debug.Log($"Mana Magic cooldown reduced to {manaCooldown}");
            }
        }

        EndCombat();
    }

    private Image FindEnemyHealthBar(GameObject enemy)
    {
        string enemyName = enemy.name; 
        GameObject healthBarContainer = GameObject.Find($"{enemyName}_Healthbar");

        if (healthBarContainer == null)
        {
            Debug.LogError($"Health bar container '{enemyName}_Healthbar' not found!");
            return null;
        }

        Image healthBar = healthBarContainer.transform.Find("Health " + enemyName)?.GetComponent<Image>();

        if (healthBar == null)
        {
            Debug.LogError($"Health bar for '{enemyName}' not found!");
        }

        return healthBar;
    }

    public void PlayerAttack(string attackType)
    {
        if (currentTurn != TurnState.PlayerTurn) return; 

        if (enemy == null)
        {
            Debug.LogWarning("Cannot attack. Enemy is null or already destroyed.");
            currentTurn = TurnState.CombatEnd; // End combat if enemy is destroyed
            return;
        }

        switch (attackType)
        {
            case "SwiftStrike":
                combatUIManager.AddToCombatLog($"Player used Swift Strike on {enemy.name}!");
                SwiftStrike();
                break;

            case "ManaMagic":
                if (manaCooldown > 0)
                {
                    combatUIManager.AddToCombatLog($"Mana Magic is on cooldown! {manaCooldown} turns remaining.");
                    return; // Exit without performing the attack
                }
                combatUIManager.AddToCombatLog($"Player used Mana Magic on {enemy.name}!");
                ManaMagic();
                manaCooldown = 3; // Reset cooldown after use
                break;

            case "DefensiveStance":
                combatUIManager.AddToCombatLog("Player used Defensive Stance and healed!");
                DefensiveStance();
                break;

            default:
                Debug.LogError($"Unknown attack type: {attackType}");
                break;
        }

        if (CheckCombatEnd()) return; // Check if combat should end after the player's turn

        currentTurn = TurnState.EnemyTurn;
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2);

        if (player == null)
        {
            Debug.LogWarning("Player is null or already destroyed. Ending combat.");
            currentTurn = TurnState.CombatEnd;
            yield break;
        }

        combatUIManager.AddToCombatLog($"{enemy.name} attacks the player!");
        DealDamage(player, 5, "EnemyAttack");

        if (CheckCombatEnd()) yield break;
        
        currentTurn = TurnState.PlayerTurn;
    }

    private void SwiftStrike()
    {
        if (enemy == null)
        {
            Debug.LogWarning("Cannot perform Swift Strike. Enemy is null or already destroyed.");
            return;
        }

        DealDamage(enemy, 10, "SwiftStrike");
    }

    private void ManaMagic()
    {
        if (enemy == null)
        {
            Debug.LogWarning("Cannot perform Mana Magic. Enemy is null or already destroyed.");
            return;
        }

        DealDamage(enemy, 18, "ManaMagic");
    }

    private void DefensiveStance()
    {
        Heal(player, 12);
    }

    private void DealDamage(GameObject target, int damage, string attackType)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null or already destroyed. Skipping damage.");
            return;
        }

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage, attackType);

            
            if (target == player)
            {
                combatUIManager.UpdateHealthBar(combatUIManager.PlayerHealthBar, targetHealth.currentHealth, targetHealth.maxHealth);
            }
            else if (target == enemy)
            {
                combatUIManager.UpdateHealthBar(combatUIManager.EnemyHealthBar, targetHealth.currentHealth, targetHealth.maxHealth);
            }
        }
        else
        {
            Debug.LogError("Target does not have a Health component.");
        }
    }

    private void Heal(GameObject target, int amount)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.Heal(amount);
        }
    }
    public void OnEnemyDeath(string enemyName)
    {
        combatUIManager.AddToCombatLog($"{enemyName} has been defeated!");
        Debug.Log($"{enemyName} has been defeated!");
    }

    private bool CheckCombatEnd()
    {
        if (player == null || enemy == null)
        {
            Debug.LogWarning("Combat cannot continue as one of the entities is missing.");
            currentTurn = TurnState.CombatEnd;
            return true;
        }

        Health playerHealth = player.GetComponent<Health>();
        Health enemyHealth = enemy.GetComponent<Health>();

        if (playerHealth.currentHealth <= 0)
        {
            Debug.Log("Player has been defeated. Combat ends.");
            currentTurn = TurnState.CombatEnd;
            return true;
        }

        if (enemyHealth.currentHealth <= 0)
        {
            Debug.Log("Enemy has been defeated. Combat ends.");
            currentTurn = TurnState.CombatEnd;
            return true;
        }

        return false;
    }

    private void EndCombat()
    {
        Debug.Log("Combat has ended.");
        combatUIManager.HideCombatUI();

        combatUIManager.StopClearingCombatLog();

        if (enemy != null)
        {
            Destroy(enemy);
            enemy = null; // Clear reference to avoid further access
            Debug.Log("Enemy destroyed and reference cleared.");
        }
    }
}
