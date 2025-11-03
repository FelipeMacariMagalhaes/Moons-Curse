using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class HouseCutsceneController : MonoBehaviour
{
    [Header("Referências da Cutscene")]
    public PlayableDirector timeline;           // Timeline da cutscene
    public PlayController playerControlManager; // Controle do player
    public CanvasGroup fadeCanvas;              // Fade UI
    public AudioSource ambientAudio;            // Som ambiente
    public AudioClip acceptLanternClip;         // Som final (quando pega lanterna)

    private bool cutsceneStarted = false;

    void Start()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 1;
            StartCoroutine(Fade(1, 0, 2f)); // Fade in suave no começo
        }

        if (ambientAudio != null)
            ambientAudio.Play();
    }

    // 🔹 Esse método será chamado pelo script do telefone
    public void OnPhoneAnswered()
    {
        if (cutsceneStarted) return;
        cutsceneStarted = true;

        Debug.Log("Telefone atendido — iniciando cutscene...");

        if (playerControlManager != null)
            playerControlManager.EnableControls(false);

        if (timeline != null)
        {
            timeline.Play();
            timeline.stopped += OnCutsceneEnd;
        }
    }

    void OnCutsceneEnd(PlayableDirector obj)
    {
        if (acceptLanternClip != null && ambientAudio != null)
            ambientAudio.PlayOneShot(acceptLanternClip);

        if (fadeCanvas != null)
            StartCoroutine(Fade(0, 1, 2.5f));

        if (playerControlManager != null)
            StartCoroutine(ReactivatePlayerAfterFade(2.5f));
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            if (fadeCanvas != null)
                fadeCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        if (fadeCanvas != null)
            fadeCanvas.alpha = endAlpha;
    }

    IEnumerator ReactivatePlayerAfterFade(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerControlManager != null)
            playerControlManager.EnableControls(true);
    }
}

