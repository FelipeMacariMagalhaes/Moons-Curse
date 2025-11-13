using UnityEngine;
public class LanternPickup : MonoBehaviour
{
 
    public FadeScreenHouse screenFader;  
    public string nextSceneName = "Principal";  

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Lanterna...");
            screenFader.FadeOutAndChangeScene(nextSceneName);
        }
    }
}

