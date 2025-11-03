using UnityEngine;
public class LanternPickup : MonoBehaviour
{
 
    public FadeScreenHouse screenFader; // arraste o objeto do fade
    public string nextSceneName = "CenaPrincipal"; // nome da próxima cena

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Lanterna pega! Iniciando fade...");
            screenFader.FadeOutAndChangeScene(nextSceneName);
        }
    }
}

