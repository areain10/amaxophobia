using System.Collections;
using UnityEngine;

public class RemnantHandler : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    [SerializeField] private GameObject remnantPrefab;

    [Header("Spawn Offset")]
    [SerializeField] private float spawnZOffset = 200f;
    [SerializeField] private float spawnYOffset = -5.5f;
    [SerializeField] private float spawnXOffset = -8f;
    [SerializeField] private int miniGameIndex = 5;

    [Header("Braking Settings")]
    [SerializeField] private float brakeSpeed = 0.5f;

    public static GameObject SpawnedRemnant { get; private set; }

    private bool hasSpawned = false;
    private bool isBraking = false;

    private EnvironmentScroller[] scrollers;

    void OnEnable()
    {
        if (hasSpawned) return;

        Transform nearestStrip = FindNearestStrip();
        if (nearestStrip == null)
        {
            Debug.LogWarning("No environment strips found. Cannot spawn Remnant.");
            return;
        }

        Vector3 spawnPos = transform.position + new Vector3(spawnXOffset, spawnYOffset, spawnZOffset);
        GameObject remnant = Instantiate(remnantPrefab, spawnPos, Quaternion.identity);
        remnant.transform.SetParent(nearestStrip);

        SpawnedRemnant = remnant;

        // Register remnant as the monster in the MiniGameManager by index
        MiniGameManager manager = FindObjectOfType<MiniGameManager>();
        if (manager != null && miniGameIndex >= 0 && miniGameIndex < manager.miniGames.Count)
        {
            manager.miniGames[miniGameIndex].monster = remnant.transform;
            Debug.Log($"Registered {remnant.name} as monster for mini-game at index {miniGameIndex}");
        }
        else
        {
            Debug.LogWarning("MiniGameManager not found or miniGameIndex out of bounds.");
        }

        // Start slowing down the environment
        StartCoroutine(SlowDownEnvironment());

        hasSpawned = true;
    }

    private Transform FindNearestStrip()
    {
        float closestDistance = float.MaxValue;
        Transform closest = null;

        foreach (var strip in GameObject.FindObjectsOfType<EnvironmentScroller>())
        {
            float dist = Vector3.Distance(transform.position, strip.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = strip.transform;
            }
        }

        return closest;
    }

    private IEnumerator SlowDownEnvironment()
    {
        if (isBraking) yield break;

        isBraking = true;
        scrollers = GameObject.FindObjectsOfType<EnvironmentScroller>();

        bool allStopped = false;
        while (!allStopped)
        {
            allStopped = true;
            foreach (var scroller in scrollers)
            {
                float currentSpeed = scroller.GetSpeed();
                if (currentSpeed > 0)
                {
                    float newSpeed = Mathf.Max(0, currentSpeed - brakeSpeed * Time.deltaTime);
                    scroller.SetSpeed(newSpeed);
                    if (newSpeed > 0)
                        allStopped = false;
                }
            }
            yield return null;
        }

        Debug.Log("Environment has fully stopped due to RemnantMiniGame.");

        if (SpawnedRemnant != null)
        {
            RemnantMover mover = SpawnedRemnant.GetComponent<RemnantMover>();
            if (mover != null)
            {
                mover.StartMovingTowardTarget();
            }
        }
    }
}
