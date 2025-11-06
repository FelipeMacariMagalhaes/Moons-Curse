using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class ScriptSkyBox : MonoBehaviour
{
    // Sky materials
    public Material skyDay;
    public Material skySunset;
    public Material skyNight;

    // UI
    public Image fadeImage;
    public TextMeshProUGUI notificationText;

    // Lights & orbits
    public Light sun;
    public Light moon;
    public Transform sunOrbit;
    public Transform moonOrbit;

    // Audio
    public AudioSource audioSource;
    public AudioClip dayToSunsetClip;
    public AudioClip sunsetToNightClip;
    public AudioClip nightToDayClip;

    // Cycle settings
    public float cycleDuration = 180f;
    public float fadeDuration = 3f;

    // Enemy (opcional)
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    private GameObject currentEnemy;

    [HideInInspector] public bool estaDeNoite = false;

    [Range(0, 24)] public float timeOfDay = 12f;
    public float daySpeed = 1f;

    // Gradients / curves
    private Gradient sunColor;
    private Gradient moonColor;
    private Gradient ambientColor;
    private AnimationCurve sunIntensity;
    private AnimationCurve moonIntensity;

    private void Awake()
    {
        CreateGradients();
        CreateCurves();
    }

    private void Start()
    {
        if (fadeImage != null)
        {
            // garante que comece transparente
            fadeImage.canvasRenderer.SetAlpha(0f);
        }
        if (notificationText != null)
        {
            notificationText.canvasRenderer.SetAlpha(0f);
        }

        StartCoroutine(CycleRoutine());
    }

    private void Update()
    {
        // Atualiza hora do dia e iluminação
        timeOfDay = (timeOfDay + Time.deltaTime * daySpeed) % 24f;
        float t = timeOfDay / 24f;

        UpdateLighting(t);
        UpdateSkybox(t);
        UpdateOrbits(t);
        AutoSwitchLights();
    }

    // --- corrotina que roda o ciclo (dia -> por-do-sol -> noite -> dia ...) ---
    IEnumerator CycleRoutine()
    {
        while (true)
        {
            // Dia -> Sunset
            yield return StartCoroutine(SkyPhase(skyDay, "O dia está terminando...", dayToSunsetClip, false, false));

            // Sunset -> Night
            yield return StartCoroutine(SkyPhase(skySunset, "A noite está chegando...", sunsetToNightClip, true, true));

            // Night -> Day
            yield return StartCoroutine(SkyPhase(skyNight, "O dia está voltando...", nightToDayClip, false, false));
        }
    }

    IEnumerator SkyPhase(Material skyMat, string noticeText, AudioClip transitionSound, bool nextIsNight, bool spawnEnemy)
    {
        // Define skybox da fase
        if (skyMat != null)
        {
            RenderSettings.skybox = skyMat;
            DynamicGI.UpdateEnvironment();
        }

        // Espera grande parte do ciclo (ajustável). Aqui distribuímos 60s para notificações/transição
        float waitBeforeNotifying = Mathf.Max(0, cycleDuration - 60f);
        yield return new WaitForSeconds(waitBeforeNotifying);

        // Notificação breve
        if (notificationText != null)
        {
            notificationText.text = noticeText;
            notificationText.CrossFadeAlpha(1f, 1f, false);
        }

        yield return new WaitForSeconds(5f);

        if (notificationText != null)
            notificationText.CrossFadeAlpha(0f, 2f, false);

        yield return new WaitForSeconds(55f);

        // Fade to black (ou para a cor da image)
        yield return StartCoroutine(Fade(true));

        // Toca som de transição
        if (audioSource && transitionSound)
            audioSource.PlayOneShot(transitionSound);

        // espera o fim do fade
        yield return new WaitForSeconds(fadeDuration);

        estaDeNoite = nextIsNight;

        HandleEnemy(spawnEnemy);

        // Fade in (volta)
        yield return StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool fadeOut)
    {
        if (fadeImage == null)
        {
            yield break;
        }

        // certificar alpha inicial
        float startAlpha = fadeImage.canvasRenderer.GetAlpha();
        float endAlpha = fadeOut ? 1f : 0f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadeImage.canvasRenderer.SetAlpha(a);
            yield return null;
        }
        fadeImage.canvasRenderer.SetAlpha(endAlpha);
    }

    void HandleEnemy(bool shouldSpawn)
    {
        if (shouldSpawn)
        {
            if (enemyPrefab != null && enemySpawnPoint != null && currentEnemy == null)
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

    // --- Gradientes e curvas de iluminação ---
    void CreateGradients()
    {
        sunColor = new Gradient();
        sunColor.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(0.05f,0.07f,0.15f), 0f),
                new GradientColorKey(new Color(1f,0.45f,0.25f), 0.23f),
                new GradientColorKey(Color.white, 0.5f),
                new GradientColorKey(new Color(1f,0.5f,0.25f), 0.73f),
                new GradientColorKey(new Color(0.05f,0.07f,0.15f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f,0f), new GradientAlphaKey(1f,1f)
            }
        );

        moonColor = new Gradient();
        moonColor.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(0.4f,0.45f,0.6f), 0f),
                new GradientColorKey(new Color(0.2f,0.25f,0.4f), 0.5f),
                new GradientColorKey(new Color(0.4f,0.45f,0.6f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f,0f), new GradientAlphaKey(1f,1f)
            }
        );

        ambientColor = new Gradient();
        ambientColor.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(0.05f,0.07f,0.12f), 0f),
                new GradientColorKey(new Color(1f,0.7f,0.5f), 0.25f),
                new GradientColorKey(new Color(1f,1f,1f), 0.5f),
                new GradientColorKey(new Color(1f,0.6f,0.4f), 0.75f),
                new GradientColorKey(new Color(0.05f,0.07f,0.12f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f,0f), new GradientAlphaKey(1f,1f)
            }
        );
    }

    void CreateCurves()
    {
        sunIntensity = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.23f, 1f),
            new Keyframe(0.5f, 1f),
            new Keyframe(0.73f, 1f),
            new Keyframe(1f, 0f)
        );

        moonIntensity = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(0.25f, 0f),
            new Keyframe(0.75f, 0f),
            new Keyframe(1f, 1f)
        );
    }

    // --- Updates de iluminação, skybox e órbita ---
    void UpdateLighting(float t)
    {
        if (sun != null && sunColor != null) sun.color = sunColor.Evaluate(t);
        if (moon != null && moonColor != null) moon.color = moonColor.Evaluate(t);
        if (ambientColor != null) RenderSettings.ambientLight = ambientColor.Evaluate(t);
        if (sun != null && sunIntensity != null) sun.intensity = sunIntensity.Evaluate(t);
        if (moon != null && moonIntensity != null) moon.intensity = moonIntensity.Evaluate(t);
    }

    void UpdateSkybox(float t)
    {
        if (t < 0.25f) RenderSettings.skybox = skyNight;
        else if (t < 0.35f) RenderSettings.skybox = skySunset;
        else if (t < 0.75f) RenderSettings.skybox = skyDay;
        else if (t < 0.85f) RenderSettings.skybox = skySunset;
        else RenderSettings.skybox = skyNight;
    }

    void UpdateOrbits(float t)
    {
        float sunAngle = t * 360f - 90f;
        float moonAngle = sunAngle + 180f;

        if (sunOrbit != null) sunOrbit.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
        if (moonOrbit != null) moonOrbit.rotation = Quaternion.Euler(moonAngle, 0f, 0f);
    }

    void AutoSwitchLights()
    {
        bool isDay = timeOfDay >= 6f && timeOfDay <= 18f;

        if (sun != null) sun.enabled = isDay;
        if (moon != null) moon.enabled = !isDay;
    }
}
