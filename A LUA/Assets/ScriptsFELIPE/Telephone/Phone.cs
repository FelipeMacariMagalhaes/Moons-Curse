using UnityEngine;

public class Phone : MonoBehaviour
{

    public DialogueData phoneDialogue;
    public bool hasBeenUsed = false;
   
   private void OnTriggerEnter(Collider coli)
   {
        if(hasBeenUsed) return;
        if(coli.CompareTag("Player"))
        {
            hasBeenUsed = true;
            DialogueManager.Instance.StartDialogue(phoneDialogue);
        }

   }
}
