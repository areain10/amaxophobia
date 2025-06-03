using UnityEngine;

public class DeadEndHandler : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject deadEndPrefab;
    [SerializeField] private GameObject deadEndSignPrefab;

    [Header("Dead End Offset")]
    [SerializeField] private float spawnZOffset = 30f;
    [SerializeField] private float spawnYOffset = -3f;

    [Header("Sign Settings")]
    [SerializeField] private Vector3 signOffset = new Vector3(-3f, 0f, 25f); // Position left of road
    [SerializeField] private Vector3 signRotation = new Vector3(0f, 90f, -90f); // Adjust as needed to stand upright and face forward

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

            // Spawn the Dead End object
            Vector3 deadEndPos = transform.position + new Vector3(0f, spawnYOffset, spawnZOffset);
            GameObject deadEnd = Instantiate(deadEndPrefab, deadEndPos, Quaternion.identity);
            deadEnd.transform.SetParent(nearestStrip);

            // Spawn and rotate the sign
            Vector3 signPos = transform.position + signOffset;
            GameObject sign = Instantiate(deadEndSignPrefab, signPos, Quaternion.Euler(signRotation));
            sign.transform.SetParent(nearestStrip);

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