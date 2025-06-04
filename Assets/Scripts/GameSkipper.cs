using UnityEngine;
using UnityEngine.Playables;

public class GameSkipper : MonoBehaviour
{
    [SerializeField] private float skipTime = 150f;

    [ContextMenu(" Fast Forward to Skip Time")]
    private void DebugFastForward()
    {
        // Call your game systems here
        Debug.Log("Fast forwarding to " + skipTime + " seconds.");

        // Example system:
        var director = GetComponent<PlayableDirector>();
        if (director != null)
        {
            director.time = skipTime;
            director.Evaluate();
        }

        // Add more system fast-forward logic as needed
    }
}
