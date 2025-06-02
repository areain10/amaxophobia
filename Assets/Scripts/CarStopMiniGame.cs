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
    }

    void OnTriggerEnter(Collider other)
    {
       
           
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); // Restart game
    }


}