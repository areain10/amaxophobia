using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [System.Serializable]
    public class MiniGameEntry
    {
        public string tag;
        public GameObject panel;
    }

    public List<MiniGameEntry> miniGames = new List<MiniGameEntry>();

    public void TryActivateMiniGame(GameObject hitObject)
    {
        string objTag = hitObject.tag;

        foreach (var miniGame in miniGames)
        {
            if (objTag == miniGame.tag)
            {
                foreach (var game in miniGames)
                {
                    game.panel.SetActive(false);
                }

                miniGame.panel.SetActive(true);
                Debug.Log($"Mini-game with tag '{objTag}' activated.");
                return;
            }  

        }

        Debug.Log($"No mini-game mapped for tag: {objTag}");
    
    }

    public void CloseAllMiniGames()
    {
        foreach (var game in miniGames)
        {
            game.panel.SetActive(false);
        }

        Debug.Log("All mini-games closed.");
    }

}
