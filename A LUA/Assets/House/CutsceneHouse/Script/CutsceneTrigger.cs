using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector timeline;

    private bool triggered = false;
    

    private void OnTriggerEnter(Collider coll)
    {
        if(!triggered && coll.CompareTag("Player"))
        {
            timeline.Play();
            triggered = true;
        }
    }
}
