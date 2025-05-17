using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WipeButtonClicker : MonoBehaviour
{

    [SerializeField] float margin = 50f;
    [SerializeField] int targetWipes = 10;
    [SerializeField] float timeLimit = 15f;

    public Button wipeButton;
    public TMP_Text counterText;
    public TMP_Text timerText;
    public GameObject miniGamePanel;
    

    private int clickCount = 0;
    private float timer = 0f;
    private bool gameActive = true;
    void Start()
    {
        wipeButton.onClick.AddListener(OnWipeClicked);
        UpdateCounterText();
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
            SceneManager.LoadScene(0);
        }

    }






    void OnWipeClicked()
    {
        if (!gameActive)
            return;
        
        clickCount++;
        UpdateCounterText();
        MoveButtonToRandomPosition();

        if (clickCount >= targetWipes)
        {
            miniGamePanel.SetActive(false);
            Debug.Log("Mini-game Complete");
            gameActive = false;
        }
    
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




    void MoveButtonToRandomPosition ()
    {
        RectTransform buttonRect = wipeButton.GetComponent<RectTransform>();
        RectTransform canvasRect = wipeButton.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        

        float maxX = canvasRect.rect.width / 2 - buttonRect.rect.width / 2 - margin;
        float maxy = canvasRect.rect.height / 2 - buttonRect.rect.height / 2 - margin;

        float randomX = Random.Range(-maxX, maxX);
        float randomy = Random.Range(-maxy, maxy);

        buttonRect.anchoredPosition = new Vector2(randomX, randomy);
    }




  
}
