using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button volumeButton;

    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (menuButton != null) menuButton.onClick.AddListener(ReturnToMainMenu);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (closeButton != null) closeButton.onClick.AddListener(TogglePauseMenu);
        if (volumeButton != null) volumeButton.onClick.AddListener(AdjustVolume);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0 : 1;
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); // Main menu scene index
    }

    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit requested.");
    }

    private void AdjustVolume()
    {
        Debug.Log("Volume button pressed (placeholder).");
    }
}

