using UnityEngine;

public class MiniGamePanelEvents : MonoBehaviour
{
    public enum MiniGameTag
    {
        WiperDisable,
        ThreeOptionsDisable,
        CarSpeedUpDisable,
        SteeringDisable,
        CarStopDisable,
        CarStartDisable
    }

    [SerializeField] private MiniGameTag panelTag;

    private EventManager eventManager;

    private void Awake()
    {
        eventManager = FindObjectOfType<EventManager>();
    }

    private void OnDisable()
    {
        if (eventManager == null)
        {
            Debug.LogWarning("EventManager not found in scene.");
            return;
        }

        switch (panelTag)
        {
            case MiniGameTag.WiperDisable:
                eventManager.TriggerAudioByIndex(8, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(5, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(9, "VoiceLinesManager");
                eventManager.TriggerCutsceneByIndex(1);
                eventManager.TriggerMonsterByIndex(1);
                break;

            case MiniGameTag.ThreeOptionsDisable:
                eventManager.TriggerAudioByIndex(10, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(11, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(12, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(13, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(13, "VoiceLinesManager");
                eventManager.TriggerCutsceneByIndex(2);
                eventManager.TriggerMonsterByIndex(2);
                eventManager.TriggerAudioByIndex(14, "VoiceLinesManager");

                break;

            case MiniGameTag.CarSpeedUpDisable:
                eventManager.TriggerAudioByIndex(15, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(16, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(17, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(18, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(19, "VoiceLinesManager");
                eventManager.TriggerMonsterByIndex(3);
                eventManager.TriggerCutsceneByIndex(3);
                eventManager.TriggerAudioByIndex(20, "VoiceLinesManager");
                break;

            case MiniGameTag.CarStopDisable:
                eventManager.TriggerAudioByIndex(21, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(22, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(23, "VoiceLinesManager");
                eventManager.TriggerAudioByIndex(24, "VoiceLinesManager");
                eventManager.TriggerCutsceneByIndex(4);
                eventManager.TriggerMonsterByIndex(4);
                break;

            case MiniGameTag.SteeringDisable:
                
                break;

            case MiniGameTag.CarStartDisable:
                
                break;

            default:
                Debug.LogWarning($"Unhandled panelTag: {panelTag}");
                break;
        }
    }
}