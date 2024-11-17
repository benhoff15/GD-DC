using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    [SerializeField] private Image playerHealthBar;
    //[SerializeField] private GameObject actionMenu;

    public Image PlayerHealthBar => playerHealthBar; // Public getter for the player health bar

    public void UpdateHealthBar(Image healthBar, float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    /*public void ShowActionMenu()
    {
        actionMenu.SetActive(true);
    }

    public void HideActionMenu()
    {
        actionMenu.SetActive(false);
    }
}
*/
    }