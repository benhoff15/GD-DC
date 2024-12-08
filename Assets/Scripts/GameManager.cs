using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerCurrentHealth = 100; // Persistent health across scenes
    public Vector3 playerPosition;       // Save player's position
    public bool isEnemyDefeated = false; // Tracks if the enemy is defeated

    public string AbyssCrawler = "AbyssCrawler";
    public string TurnBasedBattleScene = "TurnBasedBattleScene";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterCombat(GameObject player)
    {
        // Save player's current health
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerCurrentHealth = playerHealth.currentHealth;
        }

        // Save player's position
        playerPosition = player.transform.position;

        // Persist the player object
        DontDestroyOnLoad(player);

        // Transition to combat scene
        SceneManager.LoadScene(TurnBasedBattleScene);
    }

    public void ExitCombat(GameObject player)
    {
        // Transition back to the main scene
        SceneManager.LoadScene(AbyssCrawler);

        // Restore player's position after the scene loads
        StartCoroutine(RestorePlayerPosition(player));
    }

    private IEnumerator RestorePlayerPosition(GameObject player)
    {
        // Wait until the main scene is fully loaded
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == AbyssCrawler);

        // Move player back to the main scene hierarchy
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());

        // Restore player's position
        player.transform.position = playerPosition;

        // If enemy is defeated, handle their removal
        if (isEnemyDefeated)
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
                Destroy(enemy);
                Debug.Log("Enemy removed from main scene.");
            }
        }
    }
}
