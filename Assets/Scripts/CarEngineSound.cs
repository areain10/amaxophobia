using System.Collections;
using UnityEngine;

public class CarEngineSound : MonoBehaviour
{
    [SerializeField] private float delayBeforeStart = 2f;
    [SerializeField] private AudioSource engineAudioSource;

    private void Start()
    {
        StartCoroutine(PlayEngineSoundAfterDelay());
    }

    private IEnumerator PlayEngineSoundAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        if (engineAudioSource != null)
        {
            engineAudioSource.loop = true;
            engineAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Engine AudioSource not assigned on " + gameObject.name);
        }
    }
}