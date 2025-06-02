using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SteerMiniGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject carRig;
    [SerializeField] private GameObject steeringPanel;

    [Header("Movement Settings")]
    [SerializeField] private float dodgeDistance = 2f;
    [SerializeField] private float returnDelay = 1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float activationDistance = 5f;

    private Vector3 originalPosition;
    private bool isDodging = false;
    private bool rightPressed = false;

    private void Start()
    {
        originalPosition = carRig.transform.position;

        leftButton.onClick.AddListener(HandleLeft);
        rightButton.onClick.AddListener(HandleRightPress);

        StartCoroutine(CheckForActivationDistance());
    }

    private void HandleLeft()
    {
        Debug.Log("Player pressed LEFT — fail.");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void HandleRightPress()
    {
        Debug.Log("Player pressed RIGHT — preparing to dodge.");
        rightPressed = true;
        steeringPanel.SetActive(false); // Close the panel immediately
    }

    private IEnumerator CheckForActivationDistance()
    {
        while (FallenTreeHandler.SpawnedTree == null)
            yield return null;

        GameObject tree = FallenTreeHandler.SpawnedTree;

        while (true)
        {
            float dist = Vector3.Distance(carRig.transform.position, tree.transform.position);
            if (dist <= activationDistance)
            {
                if (rightPressed)
                {
                    Debug.Log("Activation distance reached — dodging.");
                    yield return StartCoroutine(Dodge());
                }
                

                yield break; // exit after handling
            }

            yield return null;
        }
    }

    private IEnumerator Dodge()
    {
        if (isDodging) yield break;

        isDodging = true;
        Vector3 targetPosition = originalPosition + Vector3.right * dodgeDistance;

        while (Vector3.Distance(carRig.transform.position, targetPosition) > 0.01f)
        {
            carRig.transform.position = Vector3.MoveTowards(carRig.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);

        while (Vector3.Distance(carRig.transform.position, originalPosition) > 0.01f)
        {
            carRig.transform.position = Vector3.MoveTowards(carRig.transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Dodge complete.");
        isDodging = false;
    }

 


}