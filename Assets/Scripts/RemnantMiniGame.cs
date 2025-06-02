using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RemnantMiniGame : MonoBehaviour
{
    [Header("UI Buttons (Ordered 1–10)")]
    [SerializeField] private List<Button> numberButtons;

    [Header("Timer Settings")]
    [SerializeField] private float timeLimit = 20f; // Time in seconds
    private float timer;
    private bool timerRunning = false;
    public GameObject miniGamePanel;
    private int currentExpectedNumber = 1;

    void OnEnable()
    {
        timer = timeLimit;
        timerRunning = true;
    }

    void Start()
    {
        for (int i = 0; i < numberButtons.Count; i++)
        {
            int number = i + 1;
            numberButtons[i].onClick.AddListener(() => OnNumberButtonPressed(number));
        }
    }

    void Update()
    {
        if (!timerRunning) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Debug.Log("Time ran out! Reloading scene.");
            SceneManager.LoadScene(0);
        }
    }

    private void OnNumberButtonPressed(int number)
    {
        if (number == currentExpectedNumber)
        {
            numberButtons[number - 1].interactable = false;
            numberButtons[number - 1].gameObject.SetActive(false);

            currentExpectedNumber++;

            if (currentExpectedNumber > 10)
            {
                miniGamePanel.SetActive(false);
                Debug.Log("RemnantMiniGame success!");
                timerRunning = false;
                // Optionally notify success here
            }
        }
        else
        {
            Debug.Log("Incorrect number! Reloading scene.");
            SceneManager.LoadScene(0);
        }
    }

    public void ResetMiniGame()
    {
        currentExpectedNumber = 1;
        timer = timeLimit;
        timerRunning = true;

        foreach (var button in numberButtons)
        {
            button.interactable = true;
            button.gameObject.SetActive(true);
        }
    }
}
