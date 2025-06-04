using UnityEngine;
using UnityEngine.SceneManagement;

public class RemnantMover : MonoBehaviour
{
    private GameObject target; // The front of the car (found automatically)
    [SerializeField] private float moveSpeed = 3f;

    private bool moving = false;

    void Start()
    {
        // Find the target GameObject by tag at start
        target = GameObject.FindGameObjectWithTag("FrontOfCar");
        if (target == null)
        {
            Debug.LogError("RemnantMover: No GameObject tagged 'FrontOfCar' found in the scene!");
        }
        else
        {
            FaceTarget(); // Face the target initially
        }
    }

    public void StartMovingTowardTarget()
    {
        if (target == null)
        {
            Debug.LogWarning("RemnantMover: Cannot start moving because target is null.");
            return;
        }

        moving = true;
        FaceTarget(); // Face the target before moving
    }

    void Update()
    {
        if (!moving || target == null)
            return;

        // Move toward the target position
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void FaceTarget()
    {
        if (target == null)
            return;

        // Determine if target is to the left or right
        float directionX = target.transform.position.x - transform.position.x;

        // Flip horizontally based on target's relative position
        if (directionX > 0)
        {
            // Face right (default rotation)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (directionX < 0)
        {
            // Face left (flip on Y or Z axis depending on your art)
            // Commonly flip Z by 180 degrees in 2D:
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FrontOfCar"))
        {
            Debug.Log("Remnant reached the car.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }
}