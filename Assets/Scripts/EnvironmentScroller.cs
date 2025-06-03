using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScroller : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float length = 1400f;

    [Header("Acceleration Settings")]
    [SerializeField] public float accelerationRate = 10f; // Speed increment per second
    [SerializeField] private float startDelay = 2f; // Delay before starting movement

    private float resetZ;
    private float targetSpeed;  // Remember original speed

    private static List<EnvironmentScroller> strips = new List<EnvironmentScroller>();

    void OnEnable()
    {
        strips.Add(this);
    }

    void OnDisable()
    {
        strips.Remove(this);
    }

    void Start()
    {
        resetZ = -length;
        targetSpeed = moveSpeed;
        moveSpeed = 0f; // Start at 0 to simulate idle

        StartCoroutine(StartupDelayAndAccelerate());
    }

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (transform.position.z <= resetZ)
        {
            float maxZ = float.MinValue;
            foreach (var strip in strips)
            {
                if (strip != this && strip.transform.position.z > maxZ)
                    maxZ = strip.transform.position.z;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ + length);
        }
    }

    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void StartAccelerating()
    {
        StopAllCoroutines();
        StartCoroutine(AccelerateToTargetSpeed());
    }

    private IEnumerator StartupDelayAndAccelerate()
    {
        yield return new WaitForSeconds(startDelay);
        yield return StartCoroutine(AccelerateToTargetSpeed());
    }

    private IEnumerator AccelerateToTargetSpeed()
    {
        while (moveSpeed < targetSpeed)
        {
            moveSpeed += accelerationRate * Time.deltaTime;
            if (moveSpeed > targetSpeed)
                moveSpeed = targetSpeed;
            yield return null;
        }
    }

    public void SetAccelerationRate(float newRate)
    {
        accelerationRate = newRate;
    }
}