using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float holdDuration = 1f;
    [SerializeField] private float downHoldDuration = 5f;

    [Header("Rotation Angles")]
    [SerializeField] private float leftAngle = -80f;
    [SerializeField] private float rightAngle = 80f;
    [SerializeField] private float downAngle = 15f;

    private Quaternion originalRotation;
    private Coroutine rotationCoroutine;
    
    void Start()
    {
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RotateRight();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RotateDown();
        }
    }

    public void RotateLeft()
    {
        StartRotation(Vector3.up * leftAngle, holdDuration);
    }

    public void RotateRight()
    {
        StartRotation(Vector3.up * rightAngle, holdDuration);
    }

    public void RotateDown()
    {
        StartRotation(Vector3.right * downAngle, downHoldDuration);
    }

    private void StartRotation(Vector3 eulerOffset, float holdTime)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        Quaternion targetRotation = Quaternion.Euler(originalRotation.eulerAngles + eulerOffset);
        rotationCoroutine = StartCoroutine(RotateToTargetAndBack(targetRotation, holdTime));
    }


    private IEnumerator RotateToTargetAndBack(Quaternion targetRotation, float holdTime)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > .1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;

        yield return new WaitForSeconds(holdTime);

        while (Quaternion.Angle(transform.rotation, originalRotation) > .1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = originalRotation;

        rotationCoroutine = null;


    }


}
