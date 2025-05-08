using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WipeButtonClicker : MonoBehaviour
{

    public Button wipeButton;
    public TMP_Text counterText;

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

        float margin = 50f;

        float maxX = canvasRect.rect.width / 2 - buttonRect.rect.width / 2 - margin;
        float maxy = canvasRect.rect.height / 2 - buttonRect.rect.height / 2 - margin;

        float randomX = Random.Range(-maxX, maxX);
        float randomy = Random.Range(-maxy, maxy);

        buttonRect.anchoredPosition = new Vector2(randomX, randomy);
    }




  
}
