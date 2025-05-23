using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Rotation Angles")]
    [SerializeField] private float leftAngle = -80f;
    [SerializeField] private float rightAngle = 80f;
    [SerializeField] private float downAngle = 15f;

    private Quaternion originalRotation;
    private Coroutine rotationCoroutine;

    private enum CameraState { Idle, Left, Right, Down}
    private CameraState currentState = CameraState.Idle;

    private bool isRotating = false;
    private bool inputEnabled = true;

    void Awake()
    {
        originalRotation = transform.rotation;
    }


    void Update()
    {

        if (!inputEnabled || isRotating) return;


        if (Input.GetKeyDown(KeyCode.A) && currentState == CameraState.Idle)
        {
            RotateTo(Vector3.up * leftAngle, CameraState.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentState == CameraState.Idle)
        {
            RotateTo(Vector3.up * rightAngle, CameraState.Right);
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentState == CameraState.Idle)
        {
            RotateTo(Vector3.right * downAngle, CameraState.Down);
        }
        else if (Input.GetKeyDown(KeyCode.D) && currentState == CameraState.Left)
        {
            RotateTo(Vector3.zero, CameraState.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.A) && currentState == CameraState.Right)
        {
            RotateTo(Vector3.zero, CameraState.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.W) && currentState == CameraState.Down)
        {
            RotateTo(Vector3.zero, CameraState.Idle);
        }
    }
   
    public void EnableInput(bool enabled)
    {
        inputEnabled = enabled;
    }

    public void RotateLeft(System.Action onComplete = null)
    {
        RotateTo(Vector3.up * leftAngle, CameraState.Left, onComplete);
    }

    public void RotateRight(System.Action onComplete = null) 
    {
        RotateTo(Vector3.up * rightAngle, CameraState.Right, onComplete);
    }

    public void RotateDown(System.Action onComplete = null)
    {
        RotateTo(Vector3.right * downAngle, CameraState.Down, onComplete);
    }

    public void ReturnToIdle(System.Action onComplete = null)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        Quaternion targetRotation = originalRotation;

        // If already close enough to original, just set state and invoke callback
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            currentState = CameraState.Idle;
            onComplete?.Invoke();
            return;
        }

        rotationCoroutine = StartCoroutine(RotateToTarget(targetRotation, CameraState.Idle, onComplete));
    }



    private void RotateTo(Vector3 eulerOffset, CameraState newState, System.Action onComplete = null)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        Quaternion targetRotation = Quaternion.Euler(originalRotation.eulerAngles + eulerOffset);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            // Already at target rotation — set state and invoke callback immediately
            currentState = newState;
            onComplete?.Invoke();
            return;
        }

        Debug.Log($"Starting rotation to offset {eulerOffset} for state {newState}");
        rotationCoroutine = StartCoroutine(RotateToTarget(targetRotation, newState, onComplete));
    }
    
    
    private IEnumerator RotateToTarget(Quaternion targetRotation, CameraState newState, System.Action onComplete = null)
    {
        isRotating = true;
        Debug.Log("Rotating to " + targetRotation.eulerAngles);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        currentState = newState;
        isRotating = false;

        onComplete?.Invoke();
    }
        

}
