using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations;

public class ScreenToRay : MonoBehaviour
{

    public MiniGameManager miniGameManager;

    [Header("Hover Label Settings")]
    [SerializeField] private TextMeshProUGUI labelUI;
   
    

 
    void Update()
    {
        HandleClick();
        HandleHover();
    }

    void HandleClick()
    {
        if(Input.GetMouseButton(0))
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(cameraRay, out RaycastHit hit))
            {
                miniGameManager.TryActivateMiniGame(hit.collider.gameObject);

            }
        }
        
        
    }


    void HandleHover()
    {
        if (miniGameManager.IsAnyMiniGameActive())
        {
            labelUI.gameObject.SetActive(false);
            return;
        }
        
        
        Ray hoverRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(hoverRay, out RaycastHit hit))
        {
            CarPartLabel part = hit.collider.GetComponent<CarPartLabel>();
            if (part != null)
            {
                labelUI.text = part.partName;
                labelUI.gameObject.SetActive(true);
                return;
            }

        }

        labelUI.gameObject.SetActive(false);
    }

}
