using UnityEngine;

public class FallenTreeHandler : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject treePrefab;

    [Header("Spawn Offsets (relative to this handler)")]
    [SerializeField] private Vector3 monsterOffset = new Vector3(-2f, 0f, 10f);
    [SerializeField] private Vector3 treeOffset = new Vector3(0f, 0f, 20f);

    public static GameObject SpawnedTree { get; private set; }

    private bool hasSpawned = false;

    void OnEnable()
    {
        if (!hasSpawned)
        {
            Transform nearestStrip = FindNearestStrip();
            if (nearestStrip == null)
            {
                Debug.LogWarning("No environment strips found. Cannot spawn objects.");
                return;
            }

            Vector3 monsterPos = transform.position + monsterOffset;
            Vector3 treePos = transform.position + treeOffset;

            GameObject monster = Instantiate(monsterPrefab, monsterPos, Quaternion.identity);
            GameObject tree = Instantiate(treePrefab, treePos, Quaternion.identity);

            monster.transform.SetParent(nearestStrip);
            tree.transform.SetParent(nearestStrip);

            SpawnedTree = tree; // store reference to tree
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