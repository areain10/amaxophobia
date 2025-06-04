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
    [SerializeField] private float accelerationRate = 10f; // How quickly car returns to speed

    [Header("Dead End Monster Settings")]
    [SerializeField] private float waitBeforeDisableTime = 2f; // How long to wait after stopping

    private bool brakeRequested = false; // player clicked brake
    private bool isBrakingNow = false;   // coroutine is running

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
        Debug.Log("Brake Requested");
        miniGamePanel.SetActive(false); // close UI immediately
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
        else
        {
            Debug.LogWarning("No spawned dead end monster found to disable.");
        }

        // Restore original speed with acceleration
        for (int i = 0; i < scrollers.Length; i++)
        {
            scrollers[i].SetAccelerationRate(accelerationRate);
            scrollers[i].SetSpeed(0); // Ensure we're starting from 0
            scrollers[i].StartAccelerating(); // Accelerate to original speed
        }

        // Wait a few seconds, then destroy all DeadEndStuff-tagged objects
        yield return new WaitForSeconds(2f); // Optional delay for smoothness

        GameObject[] deadEndObjects = GameObject.FindGameObjectsWithTag("DeadEndStuff");
        foreach (GameObject obj in deadEndObjects)
        {
            Destroy(obj);
        }

        Debug.Log("DeadEndStuff objects destroyed after mini-game.");
    }


    void OnTriggerEnter(Collider other)
    {
       
           
            UnityEngine.SceneManagement.SceneManager.LoadScene(2); // Restart game
    }


}