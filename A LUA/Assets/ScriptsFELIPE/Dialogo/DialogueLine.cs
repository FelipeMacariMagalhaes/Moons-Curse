using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;               // NPC ou Player
    [TextArea(2, 4)]
    public string subtitle;                  // legenda
    public AudioClip voiceClip;              // áudio
    public float delayAfterLine = 0.5f;      // pausa antes da próxima linha
}

[CreateAssetMenu(fileName = "NewDialogueSet", menuName = "Dialogue System/Dialogue Set")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueData nextDialogue;        
}
