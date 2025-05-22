using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSpeedMiniGame : MonoBehaviour
{
    [SerializeField] private Button gasButton;
    [SerializeField] private Button brakeButton;
    [SerializeField] private float gasSpeed = 50f;
    [SerializeField] private float brakeSpeed = 25f;
    [SerializeField] private MiniGameManager miniGameManager;
    void Start()
    {
        gasButton.onClick.AddListener(() => SetSpeedAndClose(gasSpeed));
        brakeButton.onClick.AddListener(() => SetSpeedAndClose(brakeSpeed));
    }

    private void SetSpeedAndClose(float targetSpeed)
    {
        foreach (EnvironmentScroller scroller in FindObjectsOfType<EnvironmentScroller>())
        {
            scroller.SetSpeed(targetSpeed);
        }

        if (miniGameManager != null)
        {
            miniGameManager.CloseAllMiniGames();
        }
        else
        {
            Debug.LogWarning("MiniGameManger reference not set in CarSpeedMiniGame");
        }

    }

  

}
