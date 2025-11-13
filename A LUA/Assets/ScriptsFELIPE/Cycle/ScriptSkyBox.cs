using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class ScriptSkyBox : MonoBehaviour
{
      [Header("Skyboxes")]
    public Material daySky;
    public Material sunsetSky;
    public Material nightSky;

    [Header("Lights")]
    public Light sunLight;
    public Light moonLight;

    [Header("UI")]
    public Image fadeImage;
    public TextMeshProUGUI notificationText;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip dayToSunsetClip;
    public AudioClip sunsetToNightClip;
    public AudioClip nightToDayClip;

    [Header("Cycle Settings")]
    public float cycleDuration = 180f;
    public float fadeDuration = 3f;

    [Header("Enemy Control")]
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    private GameObject currentEnemy;
    [HideInInspector] public bool estaDeNoite = false;

    [Header("Lanterna do Jogador")]
    public Light playerLantern;
    public float lanternIntensity = 3f;

    private void Start()
    {
        StartCoroutine(CycleRoutine());
    }

    IEnumerator CycleRoutine()
    {
        while (true)
        {
            // DIA
            yield return StartCoroutine(SkyPhase(daySky, "O dia está terminando...", dayToSunsetClip, false,
                1.3f, 0f, 1.2f));
            
            // TARDE
            yield return StartCoroutine(SkyPhase(sunsetSky, "A noite está chegando...", sunsetToNightClip, false,
                0.6f, 0.2f, 0.8f));
            
            // NOITE
            yield return StartCoroutine(SkyPhase(nightSky, "O dia está voltando...", nightToDayClip, true,
                0f, 0.5f, 0.2f));
        }
    }

    IEnumerator SkyPhase(Material skyMat, string noticeText, AudioClip transitionSound,
                         bool isNight, float sunIntensity, float moonIntensity, float ambientIntensity)
    {
        estaDeNoite = (skyMat == nightSky);

        RenderSettings.skybox = skyMat;
        DynamicGI.UpdateEnvironment();

        HandleEnemySpawn();
        HandleLighting(sunIntensity, moonIntensity, ambientIntensity);
        HandleLantern();

        yield return new WaitForSeconds(cycleDuration - 60f);

        notificationText.text = noticeText;
        notificationText.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(5f);
        notificationText.CrossFadeAlpha(0f, 2f, false);

        yield return new WaitForSeconds(55f);

        yield return StartCoroutine(Fade(true));

        if (audioSource && transitionSound)
            audioSource.PlayOneShot(transitionSound);

        yield return new WaitForSeconds(fadeDuration);
        yield return StartCoroutine(Fade(false));
    }

    void HandleEnemySpawn()
    {
        if (estaDeNoite)
        {
            if (enemyPrefab != null && currentEnemy == null)
            {
                currentEnemy = Instantiate(enemyPrefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
            }
        }
        else
        {
            if (currentEnemy != null)
            {
                Destroy(currentEnemy);
                currentEnemy = null;
            }
        }
    }

    void HandleLighting(float sunIntensity, float moonIntensity, float ambientIntensity)
    {
        if (sunLight != null)
            sunLight.intensity = sunIntensity;

        if (moonLight != null)
            moonLight.intensity = moonIntensity;

        RenderSettings.ambientIntensity = ambientIntensity;
    }

    void HandleLantern()
    {
        if (playerLantern == null) return;

        playerLantern.enabled = estaDeNoite;
        playerLantern.intensity = estaDeNoite ? lanternIntensity : 0f;
    }

    IEnumerator Fade(bool toBlack)
    {
        float t = 0f;
        Color c = fadeImage.color;
        float startAlpha = c.a;
        float targetAlpha = toBlack ? 1f : 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            c.a = lerp;
            fadeImage.color = c;
            yield return null;
        }
    }
}