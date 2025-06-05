using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarStopMiniGame : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private Button brakeButton;
    [SerializeField] private Button gasButton;

    [Header("Settings")]
    [SerializeField] private float activationDistance = 10f;
    [SerializeField] private float brakeSpeed = 0.5f;

    [Header("Acceleration Settings")]
    [SerializeField] private float accelerationRate = 10f;

    [Header("Dead End Monster Settings")]
    [SerializeField] private float waitBeforeDisableTime = 2f;

    [Header("Heartbeat Audio")]
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 1f;

    private bool brakeRequested = false;
    private bool isBrakingNow = false;
    private bool heartbeatStarted = false;

    private EnvironmentScroller[] scrollers;
    private float[] originalSpeeds;

    [SerializeField] private Transform carTransform;

    void Start()
    {
        brakeButton.onClick.AddListener(OnBrakePressed);
        gasButton.onClick.AddListener(OnGasPressed);

        scrollers = FindObjectsOfType<EnvironmentScroller>();
        originalSpeeds = new float[scrollers.Length];
        for (int i = 0; i < scrollers.Length; i++)
        {
            originalSpeeds[i] = scrollers[i].GetSpeed();
        }
    }

    void Update()
    {
        // Start heartbeat only once when panel is active
        if (!heartbeatStarted && miniGamePanel.activeSelf && !brakeRequested)
        {
            PlayHeartbeat();
            heartbeatStarted = true;
        }

        if (brakeRequested && DeadEndHandler.SpawnedDeadEnd != null)
        {
            float distance = Vector3.Distance(carTransform.position, DeadEndHandler.SpawnedDeadEnd.transform.position);
            if (!isBrakingNow && distance <= activationDistance)
            {
                isBrakingNow = true;
                StartCoroutine(GradualBrakeToStop());
                Debug.Log("GradualBrakeToStop Coroutine Started");
            }
        }
    }

    void OnBrakePressed()
    {
        brakeRequested = true;
        StopHeartbeat();
        Debug.Log("Brake Requested");
        miniGamePanel.SetActive(false);
    }

    void OnGasPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    IEnumerator GradualBrakeToStop()
    {
        bool allStopped = false;
        while (!allStopped)
        {
            allStopped = true;
            for (int i = 0; i < scrollers.Length; i++)
            {
                float currentSpeed = scrollers[i].GetSpeed();
                if (currentSpeed > 0)
                {
                    float newSpeed = Mathf.Max(0, currentSpeed - brakeSpeed * Time.deltaTime);
                    scrollers[i].SetSpeed(newSpeed);
                    if (newSpeed > 0) allStopped = false;
                }
            }
            yield return null;
        }

        Debug.Log("Mini-game complete: car has come to a full stop.");
        StartCoroutine(DisableDeadEndMonsterAfterDelay());
    }

    private IEnumerator DisableDeadEndMonsterAfterDelay()
    {
        yield return new WaitForSeconds(waitBeforeDisableTime);

        GameObject spawnedMonster = DeadEndHandler.SpawnedDeadEnd;

        if (spawnedMonster != null)
        {
            spawnedMonster.SetActive(false);
            Debug.Log("Spawned dead end monster disabled.");
        }

        for (int i = 0; i < scrollers.Length; i++)
        {
            scrollers[i].SetAccelerationRate(accelerationRate);
            scrollers[i].SetSpeed(0);
            scrollers[i].StartAccelerating();
        }

        yield return new WaitForSeconds(2f);

        GameObject[] deadEndObjects = GameObject.FindGameObjectsWithTag("DeadEndStuff");
        foreach (GameObject obj in deadEndObjects)
        {
            Destroy(obj);
        }

        Debug.Log("DeadEndStuff objects destroyed after mini-game.");
    }

    void OnTriggerEnter(Collider other)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    private void PlayHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatClip != null && !heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.clip = heartbeatClip;
            heartbeatAudioSource.volume = heartbeatVolume;
            heartbeatAudioSource.loop = true;
            heartbeatAudioSource.Play();
            Debug.Log("Heartbeat started");
        }
    }

    private void StopHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
            Debug.Log("Heartbeat stopped");
        }
    }
}