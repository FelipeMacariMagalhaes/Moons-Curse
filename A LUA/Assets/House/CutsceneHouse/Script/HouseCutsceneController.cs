using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class HouseCutsceneController : MonoBehaviour
{
   public static HouseCutsceneController Instance;

    [Header("Timelines")]
    public PlayableDirector cutscenePhone1;
    public PlayableDirector cutscenePhone2;
    public PlayableDirector cutsceneWeapons;

    [Header("Câmeras")]
    public GameObject playerCamera;
    public GameObject phone1Camera;
    public GameObject phone2Camera;
    public GameObject weaponsCamera;

    [Header("Zoom final")]
    public Camera phone1CamComponent;     
    public float zoomTarget = 30f;        
    public float zoomSpeed = 5f;          

    [Header("UI")]
    public CanvasGroup fadeCanvas;       
    public TextMeshProUGUI subtitleText;            

    [Header("Diálogo da cutscene 2")]
    public DialogueData dialoguePhone;   

    [Header("Som Final / Ambiência")]
    public AudioClip ambientSound;       
    public AudioClip echoEffect;         

    private AudioSource audioSource;
    private AudioSource ambientSource;

    private void Awake()
    {
        Instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.loop = true;
        ambientSource.volume = 0.3f;
    }

    private void Start()
    {
        AtivarSomenteCamera(playerCamera);
        fadeCanvas.alpha = 0;
    }

    // === CUTSCENE 1 ===
    public void StartPhoneCutscene()
    {
        PlayController.Instance.EnableControls(false);
        AtivarSomenteCamera(phone1Camera);
        cutscenePhone1.Play();
        cutscenePhone1.stopped += OnCutscenePhone1End;
    }

    private void OnCutscenePhone1End(PlayableDirector director)
    {
        cutscenePhone1.stopped -= OnCutscenePhone1End;
        StartCoroutine(PlayDialogueCoroutine(dialoguePhone));
    }

    // === CUTSCENE 2 (DIÁLOGO) ===
    IEnumerator PlayDialogueCoroutine(DialogueData data)
    {
        if (data == null || data.lines.Length == 0)
            yield break;

        AtivarSomenteCamera(phone2Camera);
        yield return new WaitForSeconds(0.5f);

        foreach (var line in data.lines)
        {
            subtitleText.text = $"{line.speakerName}: {line.subtitle}";
            if (line.voiceClip != null)
            {
                audioSource.clip = line.voiceClip;
                audioSource.Play();
            }
            yield return new WaitForSeconds(line.voiceClip ? line.voiceClip.length + line.delayAfterLine : 2f);
        }

        subtitleText.text = "";
        PlayController.Instance.EnableControls(true);
    }

    // === CUTSCENE 3 (ARMAS) ===
    public void PlayWeaponsCutscene()
    {
        PlayController.Instance.EnableControls(false);
        AtivarSomenteCamera(weaponsCamera);
        cutsceneWeapons.Play();
        cutsceneWeapons.stopped += OnCutsceneWeaponsEnd;
    }

    private void OnCutsceneWeaponsEnd(PlayableDirector director)
    {
        cutsceneWeapons.stopped -= OnCutsceneWeaponsEnd;
        StartCoroutine(FinalPhoneScene());
    }

    // === CENA FINAL COM ZOOM ===
    IEnumerator FinalPhoneScene()
    {
        AtivarSomenteCamera(phone1Camera);

        if (ambientSound != null)
        {
            ambientSource.clip = ambientSound;
            ambientSource.Play();
        }

        // fade para preto
        yield return StartCoroutine(Fade(0, 1, 2f));

        // inicia o zoom enquanto os textos aparecem
        StartCoroutine(SmoothZoom());

        // texto 1
        yield return StartCoroutine(ShowTextWithSound("O telefone é o único que posso confiar...", 3f));

        // texto 2
        yield return StartCoroutine(ShowTextWithSound("Por enquanto...", 3f));

        // fade de áudio e tela
        StartCoroutine(FadeOutAudio(ambientSource, 2f));
        yield return StartCoroutine(Fade(1, 0, 1.5f));

        SceneManager.LoadScene("Cena2");
    }

    IEnumerator ShowTextWithSound(string text, float duration)
    {
        subtitleText.text = text;
        subtitleText.canvasRenderer.SetAlpha(0f);
        subtitleText.CrossFadeAlpha(1f, 1.5f, false);

        if (echoEffect != null)
        {
            audioSource.PlayOneShot(echoEffect, 0.5f);
        }

        yield return new WaitForSeconds(duration);
        subtitleText.CrossFadeAlpha(0f, 1f, false);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            fadeCanvas.alpha = Mathf.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvas.alpha = end;
    }

    IEnumerator FadeOutAudio(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0;
        while (time < duration)
        {
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.Stop();
    }

    IEnumerator SmoothZoom()
    {
        if (phone1CamComponent == null)
            yield break;

        float startFov = phone1CamComponent.fieldOfView;
        float elapsed = 0;

        while (elapsed < zoomSpeed)
        {
            phone1CamComponent.fieldOfView = Mathf.Lerp(startFov, zoomTarget, elapsed / zoomSpeed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        phone1CamComponent.fieldOfView = zoomTarget;
    }

    void AtivarSomenteCamera(GameObject ativa)
    {
        playerCamera.SetActive(ativa == playerCamera);
        phone1Camera.SetActive(ativa == phone1Camera);
        phone2Camera.SetActive(ativa == phone2Camera);
        weaponsCamera.SetActive(ativa == weaponsCamera);
    }
}

