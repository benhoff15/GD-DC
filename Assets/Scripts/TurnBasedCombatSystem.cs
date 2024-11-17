using System.Collections;
using UnityEngine;

public class TurnBasedCombatSystem : MonoBehaviour
{
    public CombatUIManager combatUIManager;

    private GameObject player;
    private GameObject enemy;

    public enum TurnState { PlayerTurn, EnemyTurn, CombatEnd }
    private TurnState currentTurn;

    public void StartCombat(GameObject enemyObj, GameObject playerObj)
    {
        player = playerObj;
        enemy = enemyObj;

        Debug.Log("Combat started!");

        Health playerHealth = player.GetComponent<Health>();
        Health enemyHealth = enemy.GetComponent<Health>();

        combatUIManager.UpdateHealthBar(combatUIManager.PlayerHealthBar, playerHealth.currentHealth, playerHealth.maxHealth);

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
                    //combatUIManager.ShowActionMenu();
                    yield return new WaitUntil(() => PlayerActionSelected());
                    //combatUIManager.HideActionMenu();

                    DealDamage(enemy, 10);

                    if (CheckCombatEnd()) break;

                    currentTurn = TurnState.EnemyTurn;
                    break;

                case TurnState.EnemyTurn:
                    Debug.Log("Enemy's Turn");
                    yield return EnemyTurn();

                    combatUIManager.UpdateHealthBar(combatUIManager.PlayerHealthBar, player.GetComponent<Health>().currentHealth, player.GetComponent<Health>().maxHealth);

                    if (CheckCombatEnd()) break;

                    currentTurn = TurnState.PlayerTurn;
                    break;
            }
        }

        EndCombat();
    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Enemy attacks player!");
        DealDamage(player, 5);
    }

    private bool PlayerActionSelected()
    {
        return Input.GetKeyDown(KeyCode.A); // Replace with UI logic
    }

    private void DealDamage(GameObject target, int damage)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }
    }

    private bool CheckCombatEnd()
    {
        Health playerHealth = player.GetComponent<Health>();
        Health enemyHealth = enemy.GetComponent<Health>();

        if (playerHealth.currentHealth <= 0 || enemyHealth.currentHealth <= 0)
        {
            currentTurn = TurnState.CombatEnd;
            return true;
        }
        return false;
    }

    private void EndCombat()
    {
        Debug.Log("Combat has ended.");
        //combatUIManager.HideActionMenu();

        Player playerMovement = player.GetComponent<Player>();
        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }
    }
}
