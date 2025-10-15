using UnityEngine;

public class ScriptSkyBox : MonoBehaviour
{
    public Material skyDay;
    public Material skySunset;
    public Material skyNight;

    public Light sun;
    public Light moon;

    public Transform sunOrbit;
    public Transform moonOrbit;

    [Range(0, 24)] public float timeOfDay = 12f;
    public float daySpeed = 1f;

    private Gradient sunColor;
    private Gradient moonColor;
    private Gradient ambientColor;
    private AnimationCurve sunIntensity;
    private AnimationCurve moonIntensity;

    void Awake()
    {
        CreateGradients();
        CreateCurves();
    }

    void Update()
    {
        timeOfDay = (timeOfDay + Time.deltaTime * daySpeed) % 24f;
        float t = timeOfDay / 24f;

        UpdateLighting(t);
        UpdateSkybox(t);
        UpdateOrbits(t);
        AutoSwitchLights();
    }

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
                new GradientAlphaKey(1,0f), new GradientAlphaKey(1,1f)
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
                new GradientAlphaKey(1,0f), new GradientAlphaKey(1,1f)
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
                new GradientAlphaKey(1,0f), new GradientAlphaKey(1,1f)
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

    void UpdateLighting(float t)
    {
        sun.color = sunColor.Evaluate(t);
        moon.color = moonColor.Evaluate(t);
        RenderSettings.ambientLight = ambientColor.Evaluate(t);
        sun.intensity = sunIntensity.Evaluate(t);
        moon.intensity = moonIntensity.Evaluate(t);
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