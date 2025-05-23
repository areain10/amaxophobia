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

    private bool isRotating = false;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (isRotating) return;

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
   
    private void RotateTo(Vector3 eulerOffset, CameraState newState)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        Quaternion targetRotation = Quaternion.Euler(originalRotation.eulerAngles + eulerOffset);
        rotationCoroutine = StartCoroutine(RotateToTarget(targetRotation, newState));
    }
    
    
    private IEnumerator RotateToTarget(Quaternion targetRotation, CameraState newState)
    {
        isRotating = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > .1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        currentState = newState;
        isRotating = false;


    }
        


}
