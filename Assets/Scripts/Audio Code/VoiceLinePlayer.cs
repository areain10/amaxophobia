using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class VoiceLinePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private VoiceLineEntry[] voiceLineEntries;

    private Dictionary<string, AudioClip> voiceLines;

    [System.Serializable]
    public class VoiceLineEntry
    {
        public string key;
        public AudioClip clip;
    }

    void Awake()
    {
        voiceLines = new Dictionary<string, AudioClip>();
        foreach (var entry in voiceLineEntries)
        {
            if (!voiceLines.ContainsKey(entry.key))
            {
                voiceLines.Add(entry.key, entry.clip);
            }
        }
    }

    public void PlayVoiceLine(string key)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not assigned.");
            return;
        }

        if (voiceLines.TryGetValue(key, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("No voice line found for key: " + key);
        }
    }
}