using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 1f;

    [Header("Rotation Angles")]
    [SerializeField] private float leftAngle = -80f;
    [SerializeField] private float rightAngle = 80f;
    [SerializeField] private float downAngle = 15f;

    private Quaternion originalRotation;
    private Coroutine rotationCoroutine;

    private enum CameraState { Idle, Left, Right, Down}
    private CameraState currentState = CameraState.Idle;
    
    void Start()
    {
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CameraState.Idle:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    RotateToDirection(Vector3.up * leftAngle, CameraState.Left);
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    RotateToDirection(Vector3.up * rightAngle, CameraState.Right);
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                    RotateToDirection(Vector3.right * downAngle, CameraState.Down);
                break;

            case CameraState.Left:
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    ReturnToCenter();
                break;

            case CameraState.Right:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    ReturnToCenter();
                break;

            case CameraState.Down:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                    ReturnToCenter();
                break;
        }
    }
    private void RotateToDirection(Vector3 eurlerOffset, CameraState newState)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        Quaternion targetRotation = Quaternion.Euler(originalRotation.eulerAngles + eurlerOffset);
        rotationCoroutine = StartCoroutine(RotateTo(targetRotation, () => currentState = newState));
        
    }

    private void ReturnToCenter()
    {
        if(rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        rotationCoroutine = StartCoroutine(RotateTo(originalRotation, () => currentState = CameraState.Idle));
    }

    private IEnumerator RotateTo(Quaternion targetRotation, System.Action onComplete)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > .1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        rotationCoroutine = null;
        onComplete?.Invoke();
    }   


}
