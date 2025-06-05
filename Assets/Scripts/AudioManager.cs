using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioStep
    {
        public float timeToWait;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioStep> audioSteps = new List<AudioStep>();

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        Debug.Log($"Volume set to: {volume}");
    }

    public void PlayAudioByIndex(int index)
    {
        if (index < 0 || index >= audioSteps.Count)
        {
            Debug.LogWarning("Invalid audio index");
            return;
        }

        StartCoroutine(PlayAudioStep(audioSteps[index]));
    }

    private IEnumerator PlayAudioStep(AudioStep step)
    {
        yield return new WaitForSeconds(step.timeToWait);

        if (step.clip != null && audioSource != null)
        {
            audioSource.Stop(); // Stop any currently playing clip
            audioSource.clip = step.clip;
            audioSource.volume = step.volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Missing AudioClip or AudioSource");
        }
    }
}
