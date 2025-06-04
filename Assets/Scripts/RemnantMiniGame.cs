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

    // Reference to spawned Remnant
    private GameObject spawnedRemnant;

    void OnEnable()
    {
        timer = timeLimit;
        timerRunning = true;

        spawnedRemnant = RemnantHandler.SpawnedRemnant;
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

                // Disable the spawned remnant on success
                if (spawnedRemnant != null)
                {
                    spawnedRemnant.SetActive(false);
                    Debug.Log("Spawned Remnant disabled.");
                }

                // Restart environment movement by accelerating back to normal speed
                var scrollers = FindObjectsOfType<EnvironmentScroller>();
                foreach (var scroller in scrollers)
                {
                    scroller.SetAccelerationRate(40);
                    scroller.StartAccelerating();
                }
            }
        }
        else
        {
            Debug.Log("Incorrect number! Reloading scene.");
            SceneManager.LoadScene(2);
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

        if (spawnedRemnant != null)
            spawnedRemnant.SetActive(true);
    }
}