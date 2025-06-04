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

    public Button heaterButton;
    public Button acButton;
    public Button lightsButton;

    public GameObject TheLost;

    public TMP_Text feedbackText;
    public TMP_Text promptText;
    public Light carLight;

    public MiniGameManager miniGameManager;

    private bool answered = false;
    private PromptOption currentPrompt;

    [Header("Prompt List")]
    public List<PromptOption> promptOptions = new List<PromptOption>();

    void OnEnable()
    {
        SetupGame();   
    }

    void SetupGame()
    {
        currentPrompt = promptOptions[Random.Range(0, promptOptions.Count)];
        promptText.text = currentPrompt.promptText;
        feedbackText.text = "";
        answered = false;

        heaterButton.onClick.RemoveAllListeners();
        acButton.onClick.RemoveAllListeners();
        lightsButton.onClick.RemoveAllListeners();

        heaterButton.onClick.AddListener(() => CheckAnswer("Heater"));
        acButton.onClick.AddListener(() => CheckAnswer("AC"));
        lightsButton.onClick.AddListener(() => CheckAnswer("Lights"));

    }

    void CheckAnswer(string choice)
    {
        if (answered)
            return;

        answered = true;

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

    private System.Collections.IEnumerator CloseMiniGameAfterDelay(float delay)
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

    private System.Collections.IEnumerator ReturnToSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2);
    }












}


