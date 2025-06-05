using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SteerMiniGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject carRig;
    [SerializeField] private GameObject steeringPanel;

    [Header("Movement Settings")]
    [SerializeField] private float dodgeDistance = 2f;
    [SerializeField] private float returnDelay = 1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float activationDistance = 5f;

    [Header("Heartbeat Audio")]
    [SerializeField] private AudioSource heartbeatAudioSource;
    [SerializeField] private AudioClip heartbeatClip;
    [Range(0f, 1f)] public float heartbeatVolume = 1f;

    private Vector3 originalPosition;
    private bool isDodging = false;
    private bool rightPressed = false;
    private bool heartbeatStarted = false;

    private void Start()
    {
        originalPosition = carRig.transform.position;

        leftButton.onClick.AddListener(HandleLeft);
        rightButton.onClick.AddListener(HandleRightPress);

        StartCoroutine(CheckForActivationDistance());
    }

    private void Update()
    {
        // Start heartbeat when panel becomes active and player hasn't pressed right
        if (!heartbeatStarted && steeringPanel.activeSelf && !rightPressed)
        {
            PlayHeartbeat();
            heartbeatStarted = true;
        }
    }

    private void HandleLeft()
    {
        Debug.Log("Player pressed LEFT — fail.");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void HandleRightPress()
    {
        Debug.Log("Player pressed RIGHT — preparing to dodge.");
        rightPressed = true;
        StopHeartbeat();
        steeringPanel.SetActive(false);
    }

    private IEnumerator CheckForActivationDistance()
    {
        while (FallenTreeHandler.SpawnedTree == null)
            yield return null;

        GameObject tree = FallenTreeHandler.SpawnedTree;

        while (true)
        {
            float dist = Vector3.Distance(carRig.transform.position, tree.transform.position);
            if (dist <= activationDistance)
            {
                if (rightPressed)
                {
                    Debug.Log("Activation distance reached — dodging.");
                    yield return StartCoroutine(Dodge());
                }
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Dodge()
    {
        if (isDodging) yield break;

        isDodging = true;
        Vector3 targetPosition = originalPosition + Vector3.right * dodgeDistance;

        while (Vector3.Distance(carRig.transform.position, targetPosition) > 0.01f)
        {
            carRig.transform.position = Vector3.MoveTowards(carRig.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);

        while (Vector3.Distance(carRig.transform.position, originalPosition) > 0.01f)
        {
            carRig.transform.position = Vector3.MoveTowards(carRig.transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Dodge complete.");
        isDodging = false;

        yield return new WaitForSeconds(2f);

        GameObject[] desecratorObjects = GameObject.FindGameObjectsWithTag("DesecratorStuff");
        foreach (GameObject obj in desecratorObjects)
        {
            Destroy(obj);
        }

        Debug.Log("Cleaned up Desecrator-stuff (tree and monster).");
    }

    private void PlayHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatClip != null && !heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.clip = heartbeatClip;
            heartbeatAudioSource.volume = heartbeatVolume;
            heartbeatAudioSource.loop = true;
            heartbeatAudioSource.Play();
            Debug.Log("Heartbeat started");
        }
    }

    private void StopHeartbeat()
    {
        if (heartbeatAudioSource != null && heartbeatAudioSource.isPlaying)
        {
            heartbeatAudioSource.Stop();
            Debug.Log("Heartbeat stopped");
        }
    }
}