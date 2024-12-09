using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLocation : MonoBehaviour
{
    public string AbyssCrawler = "AbyssCrawler"; // Used for change scene

    void ExampleMethod()
    {
        SceneManager.LoadScene(AbyssCrawler); //for change scene
    }
}
