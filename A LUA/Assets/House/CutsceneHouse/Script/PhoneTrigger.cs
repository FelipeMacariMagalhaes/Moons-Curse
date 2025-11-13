using UnityEngine;
using UnityEngine.Playables;

public class PhoneTrigger : MonoBehaviour
{
    public PlayableDirector phoneTimeline;
    public PlayController playerControlManager;  
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            phoneTimeline.Play();  
            if (playerControlManager != null)
            {
                playerControlManager.EnableControls(false);  
            }
            triggered = true;
        }
    }
}
