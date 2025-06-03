using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{

    [System.Serializable]
    public class CutSceneEntry
    {
        public float timeToWait;
        public CameraMovementType movementType;
    }

    public enum CameraMovementType {Left, Right, Down, Back}

    [SerializeField] private CameraController cameraController;
    [SerializeField] private List<CutSceneEntry> cutSceneSteps = new List<CutSceneEntry>();

    public void PlayCutScene(int index)
    {
        if (index < 0 || index >= cutSceneSteps.Count)
        {
            Debug.LogWarning("Invalid cutscene index");
            return;
        }


        // Skip return to idle if already idle
        StartCoroutine(WaitAndExecute(index));

    }

    private IEnumerator ExecuteCutSceneStep(int index)
    {
        CutSceneEntry step = cutSceneSteps[index];
        Debug.Log($"Executing cutscene step {index}: {step.movementType}");

        System.Action onComplete = () =>
        {
            cameraController.ReturnToIdle(() =>
            {
                cameraController.EnableInput(true);
            });
        };

        switch (step.movementType)
        {
            case CameraMovementType.Left:
                cameraController.RotateLeft(onComplete);
                break;
            case CameraMovementType.Right:
                cameraController.RotateRight(onComplete);
                break;
            case CameraMovementType.Down:
                cameraController.RotateDown(onComplete);
                break;
            case CameraMovementType.Back:
                cameraController.RotateBack(onComplete);
                break;

        }

        yield break;
    }

    private IEnumerator WaitAndExecute(int index)
    {
        CutSceneEntry step = cutSceneSteps[index];
        float timeToWait = step.timeToWait;
        float timeElapsed = 0f;
        bool inputDisabled = false;
        bool returnedToIdle = false;

        // Start timer immediately
        while (timeElapsed < timeToWait)
        {
            timeElapsed += Time.deltaTime;

            // When 3 seconds are left, disable input and return to idle once
            if (!inputDisabled && timeToWait - timeElapsed <= 5f)
            {
                inputDisabled = true;
                cameraController.EnableInput(false);

                cameraController.ReturnToIdle(() =>
                {
                    returnedToIdle = true;
                });
            }

            yield return null;
        }

        // Make sure idle is finished before cutscene
        while (!returnedToIdle)
        {
            yield return null;
        }

        // Play the actual cutscene
        StartCoroutine(ExecuteCutSceneStep(index));
    }


}
