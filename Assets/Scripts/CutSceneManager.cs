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

    public enum CameraMovementType {Left, Right, Down}

    [SerializeField] private CameraController cameraController;
    [SerializeField] private List<CutSceneEntry> cutSceneSteps = new List<CutSceneEntry>();

    public void PlayCutScene(int index)
    {
        if (index < 0 || index >= cutSceneSteps.Count)
        {
            Debug.LogWarning("Invalid cutscene index");
            return;
        }

        cameraController.EnableInput(false);

        // Skip return to idle if already idle
        StartCoroutine(WaitAndExecute(index));

    }

    private IEnumerator ExecuteCutSceneStep(int index)
    {
        CutSceneEntry step = cutSceneSteps[index];
        Debug.Log($"Executing cutscene step {index} after waiting {step.timeToWait} seconds: {step.movementType}");
        yield return new WaitForSeconds(step.timeToWait);

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
        }




    }

    private IEnumerator WaitAndExecute(int index)
    {
        yield return null; // Small delay to allow all Start/Awake to run
        cameraController.ReturnToIdle(() =>
        {
            StartCoroutine(ExecuteCutSceneStep(index));
        });
    }


}
