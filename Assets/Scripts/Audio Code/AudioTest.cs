using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    public VoiceLinePlayer voiceLinePlayer;
    public string testClipKeyWord;
    public float delayTime = 5f;

    private float timer;
    private bool hasPlayedAutomatically = false;

    void Update()
    {
        // Manual trigger with E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            voiceLinePlayer?.PlayVoiceLine(testClipKeyWord);
        }

        // Automatic trigger after delay
        if (!hasPlayedAutomatically)
        {
            timer += Time.deltaTime;
            if (timer >= delayTime)
            {
                voiceLinePlayer?.PlayVoiceLine(testClipKeyWord);
                hasPlayedAutomatically = true;
            }
        }
    }
}

