using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatUIManager : MonoBehaviour
{
    public Image PlayerHealthBar; 
    public Image EnemyHealthBar;  
    public GameObject CombatUI;   

    public Button SwiftStrikeButton;
    public Button ManaMagicButton;
    public Button DefensiveStanceButton;
    public TextMeshProUGUI CombatLogText;

    private TurnBasedCombatSystem combatSystem;

    void Start()
    {
        combatSystem = FindObjectOfType<TurnBasedCombatSystem>();
        if (combatSystem == null)
        {
            Debug.LogError("TurnBasedCombatSystem not found in the scene!");
            return;
        }

        SwiftStrikeButton.onClick.AddListener(() => combatSystem.PlayerAttack("SwiftStrike"));
        ManaMagicButton.onClick.AddListener(() => combatSystem.PlayerAttack("ManaMagic"));
        DefensiveStanceButton.onClick.AddListener(() => combatSystem.PlayerAttack("DefensiveStance"));

        // Hide Combat UI at the start
        HideCombatUI();
    }

    public void ShowCombatUI()
    {
        CombatUI.SetActive(true); // Activate combat UI
        SetButtonsInteractable(true); // Enable buttons when combat UI is shown
    }

    public void HideCombatUI()
    {
        CombatUI.SetActive(false); // Deactivate combat UI
    }

    public void UpdateHealthBar(Image healthBar, int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBar.fillAmount = fillAmount;

            
            Debug.Log($"Updating Health Bar: Current = {currentHealth}, Max = {maxHealth}, Fill = {fillAmount}");
        }
        else
        {
            Debug.LogError("Health bar Image is null!");
        }
    }

    public void SetButtonsInteractable(bool interactable)
    {
        SwiftStrikeButton.interactable = interactable;
        ManaMagicButton.interactable = interactable;
        DefensiveStanceButton.interactable = interactable;
    }

    // Dynamically assign the health bar for the current enemy
    public void SetEnemyHealthBar(Image newEnemyHealthBar)
    {
        if (newEnemyHealthBar != null)
        {
            EnemyHealthBar = newEnemyHealthBar;
            Debug.Log("Enemy health bar updated dynamically.");
        }
        else
        {
            Debug.LogError("Failed to set enemy health bar. Provided Image is null!");
        }
    }
    public void AddToCombatLog(string message)
    {
        if (CombatLogText != null)
        {
            CombatLogText.text += message + "\n"; 
        }
        else
        {
            Debug.LogWarning("Combat log text is not assigned in the UI!");
        }
    }

    public void ClearCombatLog()
    {
        if (CombatLogText != null)
        {
            CombatLogText.text = ""; // Clear all previous messages
        }
    }

    public void StartClearingCombatLog(float interval)
    {
        StartCoroutine(ClearCombatLogPeriodically(interval));
    }

    private IEnumerator ClearCombatLogPeriodically(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval); // Wait for the specified interval
            ClearCombatLog(); // Clear the combat log
        }
    }
    public void StopClearingCombatLog()
    {
        StopAllCoroutines(); // Stops all active coroutines in this script
    }
}
