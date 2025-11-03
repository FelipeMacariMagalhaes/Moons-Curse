using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public GameObject dialogueBox;       // painel principal do diálogo
    public TMP_Text speakerNameText;     // nome do falante
    public TMP_Text subtitleText;        // legenda da fala
    public AudioSource audioSource;      // áudio da fala

    private bool isPlaying = false;

    void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (dialogueBox != null) dialogueBox.SetActive(false);
    }

    public bool IsDialoguePlaying() => isPlaying;

    // Inicia um diálogo qualquer
    public void StartDialogue(DialogueData dialogue)
    {
        if (isPlaying || dialogue == null) return;
        StartCoroutine(PlayDialogueCoroutine(dialogue));
    }

    private IEnumerator PlayDialogueCoroutine(DialogueData dialogue)
    {
        isPlaying = true;

        if (dialogueBox != null) dialogueBox.SetActive(true);

        foreach (var line in dialogue.lines)
        {
            // Mostra o speakerName se houver
            speakerNameText.text = string.IsNullOrEmpty(line.speakerName) ? "" : line.speakerName;
            subtitleText.text = line.subtitle;

            // Toca o áudio se houver
            if (line.voiceClip != null && audioSource != null)
            {
                audioSource.clip = line.voiceClip;
                audioSource.Play();
                yield return new WaitWhile(() => audioSource.isPlaying);
            }

            // Aguarda o delay da linha
            yield return new WaitForSeconds(line.delayAfterLine);
        }

        // Fecha a caixa
        if (dialogueBox != null) dialogueBox.SetActive(false);
        isPlaying = false;

        // Próximo diálogo automático, se houver
        if (dialogue.nextDialogue != null)
        {
            yield return new WaitForSeconds(0.2f);
            StartDialogue(dialogue.nextDialogue);
        }
    }
}
