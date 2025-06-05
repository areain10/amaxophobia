using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThreeOptionsMiniGame : MonoBehaviour
{
    [System.Serializable]
    public class PromptOption
    {
        public string promptText;
        public string correctOption;
    }

    [Header("UI & Game Objects")]
    public Button heaterButton;
    public Button acButton;
    public Button lightsButton;
    public GameObject TheLost;
    public TMP_Text feedbackText;
    public TMP_Text promptText;
    public Light carLight;
    public MiniGameManager miniGameManager;

    [Header("Prompt List")]
    public List<PromptOption> promptOptions = new List<PromptOption>();

    [Header("Heartbeat Audio")]
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [Range(0f, 1f)][SerializeField] private float heartbeatVolume = 1f;

    [Header("Special Prompt Audio")]
    [SerializeField] private AudioSource promptAudioSource;
    [SerializeField] private AudioClip heaterPromptClip;
    [SerializeField] private AudioClip acPromptClip;

    private bool answered = false;
    private PromptOption currentPrompt;

    void OnEnable()
    {
        SetupGame();
    }

    void SetupGame()
    {
        // Filter out any prompt that has "Lights" as the correct answer
        List<PromptOption> validPrompts = promptOptions.FindAll(p => p.correctOption != "Lights");

        if (validPrompts.Count == 0)
        {
            Debug.LogError("No valid prompts available (all have 'Lights' as the correct option).");
            return;
        }

        currentPrompt = validPrompts[Random.Range(0, validPrompts.Count)];
        promptText.text = currentPrompt.promptText;
        feedbackText.text = "";
        answered = false;

        heaterButton.onClick.RemoveAllListeners();
        acButton.onClick.RemoveAllListeners();
        lightsButton.onClick.RemoveAllListeners();

        heaterButton.onClick.AddListener(() => CheckAnswer("Heater"));
        acButton.onClick.AddListener(() => CheckAnswer("AC"));
        lightsButton.onClick.AddListener(() => CheckAnswer("Lights"));

        PlayHeartbeat();
        PlaySpecialPromptAudio();
    }

    void CheckAnswer(string choice)
    {
        if (answered)
            return;

        answered = true;
        StopHeartbeat();

        if (choice == currentPrompt.correctOption)
        {
            feedbackText.text = $"You please the lost, you will live for now.";

            if (choice == "Lights" && carLight != null)
                carLight.enabled = false;

            StartCoroutine(CloseMiniGameAfterDelay(4f));
        }
        else
        {
            feedbackText.text = $"You did not follow his instructions, now you will die.";
            StartCoroutine(ReturnToSceneAfterDelay(4f));
        }
    }

    private IEnumerator CloseMiniGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (TheLost != null)
        {
            TheLost.SetActive(false);
            Debug.Log("TheLost monster disabled.");
        }
        else
        {
            Debug.LogWarning("TheLost reference not assigned in ThreeOptionsMiniGame.");
        }

        if (miniGameManager != null)
        {
            miniGameManager.CloseAllMiniGames();
        }
        else
        {
            Debug.LogWarning("MiniGameManager not assigned in ThreeOptions MiniGame");
        }
    }

    private IEnumerator ReturnToSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }

    private void PlayHeartbeat()
    {
        if (heartbeatAudioSource == null || heartbeatClip == null)
            return;

        heartbeatAudioSource.clip = heartbeatClip;
        heartbeatAudioSource.loop = true;
        heartbeatAudioSource.volume = heartbeatVolume;
        heartbeatAudioSource.Play();
    }

    private void StopHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
        }
    }

    private void PlaySpecialPromptAudio()
    {
        if (promptAudioSource == null)
            return;

        if (currentPrompt.correctOption == "Heater" && heaterPromptClip != null)
        {
            promptAudioSource.clip = heaterPromptClip;
            promptAudioSource.time = 8f; // Skip first 8 seconds
            promptAudioSource.Play();
        }
        else if (currentPrompt.correctOption == "AC" && acPromptClip != null)
        {
            promptAudioSource.clip = acPromptClip;
            promptAudioSource.time = 11f; // Skip first 11 seconds
            promptAudioSource.Play();
        }
    }

    private void OnDisable()
    {
        StopHeartbeat();
        StopPromptAudio();
    }

    private void StopPromptAudio()
    {
        if (promptAudioSource != null && promptAudioSource.isPlaying)
        {
            promptAudioSource.Stop();
        }
    }



}