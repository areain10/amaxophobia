using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WipeButtonClicker : MonoBehaviour
{

    [SerializeField] float margin = 50f;
    [SerializeField] int targetWipes = 10;

    public Button wipeButton;
    public TMP_Text counterText;
    public GameObject miniGamePanel;
    

    private int clickCount = 0;
    void Start()
    {
        wipeButton.onClick.AddListener(OnWipeClicked);
        UpdateCounterText();
    }


    void OnWipeClicked()
    {
        clickCount++;
        UpdateCounterText();
        MoveButtonToRandomPosition();

        if (clickCount >= targetWipes)
        {
            miniGamePanel.SetActive(false);
            Debug.Log("Mini-game Complete");
        }
    
    }

    void UpdateCounterText()
    {
        if (counterText != null)
            counterText.text = $"Wipes: {clickCount}";
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
