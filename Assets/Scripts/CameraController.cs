using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 5f;

    [SerializeField] float pitch = 0f;
    [SerializeField] float yaw = 0f;

    [SerializeField] float clampMin = -80f;
    [SerializeField] float clampMax = 80f;

    void Update()
    {
        ProcessRotation();
    }

    void ProcessRotation()
    {
        float xRotation = Input.GetAxis("Horizontal");
        float yRotation = Input.GetAxis("Vertical");

        yaw += xRotation * rotationSpeed * Time.deltaTime;
        pitch -= yRotation * rotationSpeed * Time.deltaTime;


        float clampedPitch = Mathf.Clamp(pitch, clampMin, clampMax);

        transform.localRotation = Quaternion.Euler(clampedPitch, yaw, 0f);
    }

}

