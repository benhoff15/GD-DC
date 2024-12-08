using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLocation : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance != null && SceneManager.GetActiveScene().name == GameManager.instance.AbyssCrawler)
        {
            transform.position = GameManager.instance.playerPosition;
        }
    }

}
