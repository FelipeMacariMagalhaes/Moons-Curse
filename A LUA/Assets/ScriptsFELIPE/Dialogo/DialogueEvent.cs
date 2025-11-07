using UnityEngine;
using UnityEngine.Events;

public class DialogueEvent : MonoBehaviour
{
    public UnityEvent onDialogueEnd;
    
    public void TriggerEvent(){
        onDialogueEnd.Invoke();
    }
}
