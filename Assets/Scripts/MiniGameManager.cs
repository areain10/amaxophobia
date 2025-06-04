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
        public Transform monster;

        [HideInInspector] public bool waitingForClick = false;
        [HideInInspector] public bool miniGameStarted = false;
        [HideInInspector] public float timer = 0f;
        [SerializeField] public float timerDuration = 10f;
    }

    [Header("Mini-Games")]
    public List<MiniGameEntry> miniGames = new List<MiniGameEntry>();
    [SerializeField] private float activationRange = 5f;

    [Header("Heartbeat Audio")]
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [SerializeField, Range(0f, 1f)] private float heartbeatVolume = 1f; // Volume control in Inspector

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (heartbeatAudioSource != null)
        {
            heartbeatAudioSource.volume = heartbeatVolume;
        }
    }

    private void Update()
    {
        bool anyWaitingForClick = false;

        foreach (var miniGame in miniGames)
        {
            if (miniGame.monster == null || miniGame.miniGameStarted)
                continue;

            if (!miniGame.monster.gameObject.activeInHierarchy)
                continue;

            GameObject triggerObject = GameObject.FindGameObjectWithTag(miniGame.tag);
            if (triggerObject == null)
                continue;

            float distance = Vector3.Distance(miniGame.monster.position, triggerObject.transform.position);

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
                anyWaitingForClick = true;

                if (miniGame.timer <= 0f)
                {
                    Debug.Log($"Player failed to click '{miniGame.tag}' in time.");
                    StopHeartbeat();
                    SceneManager.LoadScene(0);
                }
            }
        }

        if (anyWaitingForClick)
        {
            PlayHeartbeat();
        }
        else
        {
            StopHeartbeat();
        }
    }

    public void TryActivateMiniGame(GameObject hitObject)
    {
        string objTag = hitObject.tag;

        foreach (var miniGame in miniGames)
        {
            if (objTag == miniGame.tag && miniGame.waitingForClick)
            {
                miniGame.waitingForClick = false;
                miniGame.miniGameStarted = true;

                foreach (var game in miniGames)
                {
                    game.panel.SetActive(false);
                }

                miniGame.panel.SetActive(true);
                Debug.Log($"Mini-game with tag '{objTag}' activated.");
                StopHeartbeat();
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
        StopHeartbeat();
        Debug.Log("All mini-games closed.");
    }

    private void PlayHeartbeat()
    {
        if (heartbeatAudioSource == null || heartbeatClip == null)
            return;

        if (!heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.clip = heartbeatClip;
            heartbeatAudioSource.loop = true;
            heartbeatAudioSource.volume = heartbeatVolume;
            heartbeatAudioSource.Play();
        }
    }

    private void StopHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
        }
    }
}