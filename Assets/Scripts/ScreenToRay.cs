using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenToRay : MonoBehaviour
{

    public MiniGameManager miniGameManager;
    

 
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            FireScreenRay();
        }
    }

    void FireScreenRay()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(cameraRay, out RaycastHit hit))
        {
            miniGameManager.TryActivateMiniGame(hit.collider.gameObject);
            
        }
    }


}
