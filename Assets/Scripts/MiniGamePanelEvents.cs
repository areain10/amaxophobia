using UnityEngine;

public class MiniGamePanelEvents : MonoBehaviour
{
    private EventManager eventManager;

    private void Awake()
    {
        eventManager = FindObjectOfType<EventManager>(); // Or use serialized field or tag system
    }

    private void OnDisable()
    {
        if (eventManager != null)
        {
            
            eventManager.TriggerAudioByIndex(5, "VoiceLinesManager");
        }
        else
        {
            Debug.LogWarning("EventManager not found in scene.");
        }
    }
}