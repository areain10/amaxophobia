using UnityEngine;

public class DeadEndHandler : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject deadEndPrefab;

    [Header("Spawn Offset (only Z-axis used)")]
    [SerializeField] private float spawnZOffset = 30f;
    [SerializeField] private float spawnYOffset = -3f;
    public static GameObject SpawnedDeadEnd { get; private set; }

    private bool hasSpawned = false;

    void OnEnable()
    {
        if (!hasSpawned)
        {
            Transform nearestStrip = FindNearestStrip();
            if (nearestStrip == null)
            {
                Debug.LogWarning("No environment strips found. Cannot spawn DeadEnd.");
                return;
            }

            Vector3 spawnPos = transform.position + new Vector3(0f, spawnYOffset, spawnZOffset);

            GameObject deadEnd = Instantiate(deadEndPrefab, spawnPos, Quaternion.identity);
            deadEnd.transform.SetParent(nearestStrip);

            SpawnedDeadEnd = deadEnd;
            hasSpawned = true;
        }
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
}
