using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WiperMiniGame : MonoBehaviour
{
    [Header("Wipe Mini-Game Settings")]
    [SerializeField] float margin = 50f;
    [SerializeField] int targetWipes = 10;
    [SerializeField] float timeLimit = 15f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wipeClip;

    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [SerializeField, Range(0f, 1f)] private float heartbeatVolume = 1f; // Volume control

    private bool playFirstHalf = true;

    [Header("References")]
    public Button wipeButton;
    public TMP_Text counterText;
    public TMP_Text timerText;
    public GameObject miniGamePanel;
    public GameObject linkedMonster;
    public Transform wiperArm;

    private Quaternion leftRotation;
    private Quaternion rightRotation;
    private bool rotating = false;
    private float rotationSpeed = 300f;
    private bool rotateToLeft = true;

    private int clickCount = 0;
    private float timer = 0f;
    private bool gameActive = true;

    void Start()
    {
        wipeButton.onClick.AddListener(OnWipeClicked);
        UpdateCounterText();
        UpdateTimerText();

        if (wiperArm != null)
        {
            rightRotation = wiperArm.localRotation;
            leftRotation = Quaternion.Euler(wiperArm.localEulerAngles + new Vector3(0, 0, 100f));
        }

        // Start heartbeat loop when panel is active
        if (miniGamePanel.activeInHierarchy && heartbeatAudioSource != null && heartbeatClip != null)
        {
            heartbeatAudioSource.clip = heartbeatClip;
            heartbeatAudioSource.loop = true;
            heartbeatAudioSource.volume = heartbeatVolume;
            heartbeatAudioSource.Play();
        }
    }

    void Update()
    {
        if (!gameActive)
            return;

        timer += Time.deltaTime;
        float remainingTime = Mathf.Max(0, timeLimit - timer);
        UpdateTimerText(remainingTime);

        if (timer >= timeLimit)
        {
            Debug.Log("Time's up! Returning to main menu");
            gameActive = false;
            StopHeartbeat();
            SceneManager.LoadScene(2);
        }

        if (rotating && wiperArm != null)
        {
            Quaternion target = rotateToLeft ? leftRotation : rightRotation;
            wiperArm.localRotation = Quaternion.RotateTowards(wiperArm.localRotation, target, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(wiperArm.localRotation, target) < 0.5f)
            {
                rotating = false;
                rotateToLeft = !rotateToLeft;
            }
        }
    }

    void OnWipeClicked()
    {
        if (!gameActive)
            return;

        clickCount++;
        UpdateCounterText();
        MoveButtonToRandomPosition();
        rotating = true;

        PlayWipeSound();

        if (clickCount >= targetWipes)
        {
            miniGamePanel.SetActive(false);
            StopHeartbeat();
            Debug.Log("Mini-game Complete");

            if (linkedMonster != null)
            {
                linkedMonster.SetActive(false);
                Debug.Log("Monster Disabled");
            }

            gameActive = false;
        }
    }

    void PlayWipeSound()
    {
        if (audioSource == null || wipeClip == null)
            return;

        float halfDuration = wipeClip.length / 2f;

        if (audioSource.isPlaying)
            audioSource.Stop();

        if (playFirstHalf)
        {
            StartCoroutine(PlayPartialClip(0f, halfDuration));
        }
        else
        {
            StartCoroutine(PlayPartialClip(halfDuration, halfDuration));
        }

        playFirstHalf = !playFirstHalf;
    }

    IEnumerator PlayPartialClip(float startTime, float duration)
    {
        audioSource.clip = wipeClip;
        audioSource.time = startTime;
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }

    void UpdateCounterText()
    {
        if (counterText != null)
            counterText.text = $"Wipes: {clickCount}/{targetWipes}";
    }

    void UpdateTimerText(float timeLeft = -1f)
    {
        if (timerText != null)
        {
            if (timeLeft < 0f)
                timeLeft = timeLimit;

            timerText.text = $"Time Left: {timeLeft:F1}s";
        }
    }

    void MoveButtonToRandomPosition()
    {
        RectTransform buttonRect = wipeButton.GetComponent<RectTransform>();
        RectTransform canvasRect = wipeButton.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        float maxX = canvasRect.rect.width / 2 - buttonRect.rect.width / 2 - margin;
        float maxY = canvasRect.rect.height / 2 - buttonRect.rect.height / 2 - margin;

        float randomX = Random.Range(-maxX, maxX);
        float randomY = Random.Range(-maxY, maxY);

        buttonRect.anchoredPosition = new Vector2(randomX, randomY);
    }

    void StopHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
        }
    }
}