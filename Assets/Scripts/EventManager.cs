using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private CutSceneManager cutSceneManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private MonsterManager monsterManager;

    [Header("Cutscene Sequence")]
    [Tooltip("List of cutscene indices to player in order or when called")]
    public List<int> cutsceneSequence = new List<int>();

    

    private void Start()
    {
         TriggerAudioByIndex(0, "VoiceLinesManager");
         TriggerAudioByIndex(1, "VoiceLinesManager");
         TriggerCutsceneByIndex(0);
         TriggerMonsterByIndex(0);
         TriggerAudioByIndex(2, "VoiceLinesManager"); 
         TriggerAudioByIndex(0, "CarSFXManager");
         TriggerAudioByIndex(1, "CarSFXManager");
         TriggerAudioByIndex(6, "VoiceLinesManager");
         TriggerAudioByIndex(4, "VoiceLinesManager");
         TriggerAudioByIndex(7, "VoiceLinesManager");
         TriggerAudioByIndex(5, "CarSFXManager");
         TriggerAudioByIndex(3, "VoiceLinesManager");
        
        


    }



    /// <summary>
    /// Triggers a single cutscene by index.
    /// </summary>
    public void TriggerCutsceneByIndex(int index)
    {
        if (cutSceneManager != null)
        {
            cutSceneManager.PlayCutScene(index);
        }
        else
        {
            Debug.LogWarning("CutSceneManger not assigned");
        }
    }
   
    /// <summary>
    /// Triggers all cutscene steps in the predefined sequence list.
    /// </summary>
    public void TriggerCutSceneSequence()
    {
        if (cutSceneManager == null)
        {
            Debug.LogWarning("CutSceneManager not assigned.");
            return;
        }

        StartCoroutine(PlaySequenceCoroutine());
    }

    /// <summary>
    /// Triggers multiple cutscens using a custom list of indices.
    /// </summary>
    public void TriggerMultipleCutscenes(List<int> indices)
    {
        if (cutSceneManager == null)
        {
            Debug.LogWarning("CutSceneManager not assigned");
            return;
        }

        StartCoroutine(PlaySequenceCoroutine());
    }


    ///<summary>
    ///Coroutine to play cutscenes using the default cutsceneSequence list.
    /// </summary>
    private IEnumerator PlaySequenceCoroutine()
    {
        foreach (int index in cutsceneSequence)
        {
            cutSceneManager.PlayCutScene(index);
            yield return null;
        }

    }
    
    /// <summary>
    /// Coroutine to play cutscenes from a provided custom list of indices.
    /// </summary>
    private IEnumerator PlaySequenceCoroutine(List<int> indices)
    {
        foreach (int index in indices)
        {
            cutSceneManager.PlayCutScene(index);
            yield return null;

        }
    }


    /// <summary>
    /// Triggers a single audio step by index, using the GameObject tag to find the appropriate AudioManager.
    /// </summary>
    public void TriggerAudioByIndex(int index, string managerTag)
    {
        GameObject managerObject = GameObject.FindGameObjectWithTag(managerTag);
        if (managerObject == null)
        {
            Debug.LogWarning($"No GameObject found with tag '{managerTag}'");
            return;
        }

        AudioManager targetManager = managerObject.GetComponent<AudioManager>();
        if (targetManager == null)
        {
            Debug.LogWarning($"No AudioManager component found on GameObject with tag '{managerTag}'");
            return;
        }

        targetManager.PlayAudioByIndex(index);
    }

    public void TriggerMonsterByIndex(int index)
    {
        if (monsterManager != null)
        {
            monsterManager.TriggerMonsterByIndex(index);
        }
        else
        {
            Debug.LogWarning("MonsterManager not assigned");
        }
    }





}
