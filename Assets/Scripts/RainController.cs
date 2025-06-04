using System.Collections;
using UnityEngine;

public class RainController : MonoBehaviour
{
    [System.Serializable]
    public class RainStep
    {
        public float timeToWait;
        public float rainDuration;
    }

    [SerializeField] private GameObject rainEffect; // Assign your rain particle system here
    [SerializeField] private RainStep rainStep;

    void Start()
    {
        StartCoroutine(HandleRainSequence());
    }

    private IEnumerator HandleRainSequence()
    {
        // Wait before starting the rain
        yield return new WaitForSeconds(rainStep.timeToWait);

        // Start the rain
        if (rainEffect != null)
            rainEffect.SetActive(true);

        // Wait for rain duration
        yield return new WaitForSeconds(rainStep.rainDuration);

        // Stop the rain
        if (rainEffect != null)
            rainEffect.SetActive(false);
    }
}