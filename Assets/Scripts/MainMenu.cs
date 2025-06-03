using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayPanel;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void ShowHowToPlay()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(true);
    }

    public void HideHowToPlay()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }
}
