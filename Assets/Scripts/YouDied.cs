using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouDied : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button returnToMenuButton;
    [SerializeField] private Button quitButton;

    void Start()
    {
        tryAgainButton.onClick.AddListener(LoadGameScene);
        returnToMenuButton.onClick.AddListener(LoadMenuScene);
        quitButton.onClick.AddListener(QuitGame);
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(1); // Main game scene
    }

    void LoadMenuScene()
    {
        SceneManager.LoadScene(0); // Menu scene
    }

    void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}
