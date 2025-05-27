using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [System.Serializable]
    public class MonsterStep
    {
        public float timeToWait;
        public GameObject monster;
    }

    [Header("Monster Sequence Steps")]
    [SerializeField] private List<MonsterStep> monsterSteps = new List<MonsterStep>();

    /// <summary>
    /// Enables the monster at the specified index after a delay.
    /// </summary>
    public void TriggerMonsterByIndex(int index)
    {
        if (index < 0 || index >= monsterSteps.Count)
        {
            Debug.LogWarning("Invalid monster step index.");
            return;
        }

        StartCoroutine(EnableMonsterAfterDelay(monsterSteps[index]));
    }

    private IEnumerator EnableMonsterAfterDelay(MonsterStep step)
    {
        yield return new WaitForSeconds(step.timeToWait);

        if (step.monster != null)
        {
            step.monster.SetActive(true);
            Debug.Log($"Monster '{step.monster.name}' enabled after {step.timeToWait} seconds.");
        }
        else
        {
            Debug.LogWarning("Monster GameObject is not assigned in step.");
        }
    }
}

