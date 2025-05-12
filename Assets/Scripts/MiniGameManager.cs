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

    [Header("Monster Proximity Settings")]
    [SerializeField] private Transform monsterTransform;
    [SerializeField] private float activationRange = 5f;




    public void TryActivateMiniGame(GameObject hitObject)
    {
        string objTag = hitObject.tag;

        foreach (var miniGame in miniGames)
        {
            if (objTag == miniGame.tag)
            {
                // Check if the monster is close enough
                if (monsterTransform != null)
                {
                    float distance = Vector3.Distance(hitObject.transform.position, monsterTransform.position);
                    if (distance > activationRange)
                    {
                        Debug.Log("Monster is too far away to activate mini-game.");
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning("Monster Transform not assigned in MiniGameManager.");
                    return;
                }

                // Close other mini-game panels
                foreach (var game in miniGames)
                {
                    game.panel.SetActive(false);
                }

                // Activate the matching panel
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
