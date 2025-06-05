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

    [Header("Heartbeat Settings")]
    [SerializeField] private AudioSource heartbeatSource;
    [SerializeField] private AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 0.8f;
    private bool heartbeatPlaying = false;

    public GameObject miniGamePanel;
    private int currentExpectedNumber = 1;

    private GameObject spawnedRemnant;

    void Awake()
    {
        if (heartbeatSource != null && heartbeatClip != null)
        {
            heartbeatSource.clip = heartbeatClip;
            heartbeatSource.loop = true;
            heartbeatSource.volume = heartbeatVolume;
        }
    }

    void OnEnable()
    {
        timer = timeLimit;
        timerRunning = true;

        spawnedRemnant = RemnantHandler.SpawnedRemnant;
        Debug.Log("RemnantMiniGame OnEnable called");

        StartHeartbeat();
    }

    void OnDisable()
    {
        StopHeartbeat();
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
                timerRunning = false;

                if (spawnedRemnant != null)
                    spawnedRemnant.SetActive(false);

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
            SceneManager.LoadScene(2);
        }
    }

    private void StartHeartbeat()
    {
        if (!heartbeatPlaying && heartbeatSource != null && heartbeatClip != null)
        {
            heartbeatSource.volume = heartbeatVolume;
            heartbeatSource.Play();
            heartbeatPlaying = true;
        }
    }

    private void StopHeartbeat()
    {
        if (heartbeatPlaying && heartbeatSource != null)
        {
            heartbeatSource.Stop();
            heartbeatPlaying = false;
        }
    }
}