using UnityEngine;
using UnityEngine.Playables;

public class PhoneTrigger : MonoBehaviour
{
    public PlayableDirector phoneTimeline;
    public PlayController playerControlManager; // arraste seu PlayerControlManager aqui
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            phoneTimeline.Play(); // ativa Timeline do close do telefone
            if (playerControlManager != null)
            {
                playerControlManager.EnableControls(false); // trava o player
            }
            triggered = true;
        }
    }
}
