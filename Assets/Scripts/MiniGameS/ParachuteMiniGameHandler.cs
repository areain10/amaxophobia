using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParachuteMiniGameHandler : MonoBehaviour
{
    [Header("UI & Game Settings")]
    [SerializeField] private Button gasButton;
    [SerializeField] private Button brakeButton;
    [SerializeField] private float delayBeforeOutcome = 1f;
    [SerializeField] private float successSpeed = 20f;
    [SerializeField] private GameObject monsterToDisable;
    public MiniGameManager miniGameManager;

    [Header("Heartbeat Audio")]
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 1f;

    private bool completed = false;

    private void OnEnable()
    {
        completed = false;

        gasButton.onClick.RemoveAllListeners();
        brakeButton.onClick.RemoveAllListeners();

        gasButton.onClick.AddListener(OnGasPressed);
        brakeButton.onClick.AddListener(OnBrakePressed);

        PlayHeartbeat();
    }

    private void PlayHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatClip != null)
        {
            heartbeatAudioSource.clip = heartbeatClip;
            heartbeatAudioSource.volume = heartbeatVolume;
            heartbeatAudioSource.loop = true;
            heartbeatAudioSource.Play();
        }
    }

    private void StopHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
        }
    }

    private void OnGasPressed()
    {
        if (completed) return;
        completed = true;
        StopHeartbeat();

        // Speed up all EnvironmentScrollers
        var scrollers = FindObjectsOfType<EnvironmentScroller>();
        foreach (var scroller in scrollers)
        {
            scroller.SetSpeed(successSpeed);
        }

        StartCoroutine(SuccessSequence());
    }

    private void OnBrakePressed()
    {
        if (completed) return;
        completed = true;
        StopHeartbeat();

        StartCoroutine(FailureSequence());
    }

    private IEnumerator SuccessSequence()
    {
        yield return new WaitForSeconds(delayBeforeOutcome);

        if (monsterToDisable != null)
            monsterToDisable.SetActive(false);

        if (miniGameManager != null)
            miniGameManager.CloseAllMiniGames();
        else
            Debug.LogWarning("MiniGameManager not assigned in ParachuteMiniGameHandler.");
    }

    private IEnumerator FailureSequence()
    {
        yield return new WaitForSeconds(delayBeforeOutcome);
        SceneManager.LoadScene(2);
    }
}