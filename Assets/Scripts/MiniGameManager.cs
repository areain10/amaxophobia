using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [System.Serializable]
    public class MiniGameEntry
    {
        public string tag;
        public GameObject panel;
        public Transform monster; // specific monster linked to this mini-game
    }

    [SerializeField] private float activationRange = 5f;
    public List<MiniGameEntry> miniGames = new List<MiniGameEntry>();

    public void TryActivateMiniGame(GameObject hitObject)
    {
        string objTag = hitObject.tag;

        foreach (var miniGame in miniGames)
        {
            if (objTag == miniGame.tag)
            {
                if (miniGame.monster == null)
                {
                    Debug.LogWarning($"No monster assigned for mini-game with tag '{miniGame.tag}'");
                    return;
                }

                float distance = Vector3.Distance(hitObject.transform.position, miniGame.monster.position);
                if (distance > activationRange)
                {
                    Debug.Log($"Monster too far to activate mini-game '{miniGame.tag}'. Distance: {distance}");
                    return;
                }

                // Disable all panels first
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
