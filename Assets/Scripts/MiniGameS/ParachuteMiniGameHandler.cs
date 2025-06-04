using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParachuteMiniGameHandler : MonoBehaviour
{
    [SerializeField] private Button gasButton;
    [SerializeField] private Button brakeButton;
    [SerializeField] private float delayBeforeOutcome = 1f;
    [SerializeField] private float successSpeed = 20f;
    [SerializeField] private GameObject monsterToDisable;
    public MiniGameManager miniGameManager;

    private bool completed = false;

    private void OnEnable()
    {
        completed = false;

        gasButton.onClick.RemoveAllListeners();
        brakeButton.onClick.RemoveAllListeners();

        gasButton.onClick.AddListener(OnGasPressed);
        brakeButton.onClick.AddListener(OnBrakePressed);
    }

    private void OnGasPressed()
    {
        if (completed) return;
        completed = true;

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
