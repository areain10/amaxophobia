using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;
    
    [System.Serializable]
    public class MiniGameEntry
    {
        public string tag;
        public GameObject panel;
        public Transform monster; // specific monster linked to this mini-game

        [HideInInspector] public bool waitingForClick = false;
        [HideInInspector] public bool miniGameStarted = false;
        [HideInInspector] public float timer = 0f;
        [SerializeField]  public float timerDuration = 10f;
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        foreach (var miniGame in miniGames)
        {
            if (miniGame.monster == null || miniGame.miniGameStarted) 
                continue;

            if (!miniGame.monster.gameObject.activeInHierarchy) 
                continue;
            
            GameObject triggerObject = GameObject.FindGameObjectWithTag(miniGame.tag);
            if (triggerObject == null)
                continue;
            
            float distance = Vector3.Distance(miniGame.monster.position, GameObject.FindGameObjectWithTag(miniGame.tag).transform.position);

            if (distance <= activationRange)
            {
                if (!miniGame.waitingForClick)
                {
                    miniGame.waitingForClick = true;
                    miniGame.timer = miniGame.timerDuration;
                    Debug.Log($"Timer of '{miniGame.timer}' started for mini-game click '{miniGame.tag}'");
                }

            }

            if (miniGame.waitingForClick)
            {
                miniGame.timer -= Time.deltaTime;

                if (miniGame.timer <= 0f)
                {
                    Debug.Log($"Player failed to click '{miniGame.tag}' in time.");
                    SceneManager.LoadScene(0);
                }

            }


        }
    }






    [SerializeField] private float activationRange = 5f;
    public List<MiniGameEntry> miniGames = new List<MiniGameEntry>();

    public void TryActivateMiniGame(GameObject hitObject)
    {
        string objTag = hitObject.tag;

        foreach (var miniGame in miniGames)
        {
            if (objTag == miniGame.tag && miniGame.waitingForClick)
            {
                if (!miniGame.waitingForClick)
                {
                    Debug.Log($"Clicked '{objTag}', but not within range or before timer started.");
                    return;
                }

                miniGame.waitingForClick = false;
                miniGame.miniGameStarted = true;

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

    public void SetMonsterForMiniGame(string tag, Transform monsterTransform)
    {
        foreach (var miniGame in miniGames)
        {
            if (miniGame.tag == tag)
            {
                miniGame.monster = monsterTransform;
                Debug.Log($"Monster for mini-game '{tag}' set to {monsterTransform.name}");
                break;
            }
        }
    }
    public bool IsAnyMiniGameActive()
    {
        foreach (var game in miniGames)
        {
            if (game.panel.activeSelf)
                return true;
        }
        return false;
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
