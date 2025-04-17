using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameSceneLoad : MonoBehaviour, IInteractable 
{

    public int sceneNumberToLoad;
    public void Interact()
    {
        SceneManager.LoadScene(sceneNumberToLoad);
    }
}
