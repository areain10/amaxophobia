using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScroller : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float length = 1400f;

    private float resetZ;

    private static List<EnvironmentScroller> strips = new List<EnvironmentScroller>();
    void OnEnable()
    {
        strips.Add(this);
    }

    void OnDisbable()
    {
        strips.Remove(this);
    }

    void Start()
    {
        resetZ = -length;
    }


    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        Debug.Log("Plane Z: " +  transform.position.z);

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



}
