using UnityEngine;
using TMPro;

public class LanternaController : MonoBehaviour
{
    private Light light;
    private AudioSource audioSource;

    [Header("Sons")]
    public AudioClip somLigar;
    public AudioClip somDesligar;

    [Header("UI")]
    public TextMeshProUGUI tmpBattery;

    [Header("Configuração da Luz")]
    public float minSpotAngle = 5f;
    public float maxSpotAngle = 70f;
    public float multiplier = 5f;

    [Header("Bateria")]
    public float multiplierReduceBattery = 10f;
    private float batteryValue = 100f;

    void Start()
    {
        // Tenta achar automaticamente os componentes
        if (light == null)
            light = GetComponentInChildren<Light>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        // Mensagens de aviso no Console, se algo estiver faltando
        if (light == null)
            Debug.LogWarning(" Nenhum componente Light encontrado como filho de " + gameObject.name);
        if (audioSource == null)
            Debug.LogWarning("Nenhum AudioSource encontrado em " + gameObject.name);

        SetUI();
    }

    void Update()
    {
        // Clique esquerdo para ligar/desligar
        if (Input.GetMouseButtonDown(0))
            ToggleFlashlight();

        if (light != null && light.enabled)
        {
            Focar();
            ReduceBattery();
        }

        SetUI();
    }

    void ToggleFlashlight()
    {
        if (batteryValue <= 0 || light == null)
        {
            if (light != null)
                light.enabled = false;
            return;
        }

        light.enabled = !light.enabled;

        if (audioSource != null)
            audioSource.PlayOneShot(light.enabled ? somLigar : somDesligar);
    }

    void ReduceBattery()
    {
        batteryValue = Mathf.Clamp(batteryValue - multiplierReduceBattery * Time.deltaTime, 0, 100);

        if (batteryValue <= 0 && light != null)
            light.enabled = false;
    }

    void Focar()
    {
        if (light == null) return;
        light.spotAngle = Mathf.Clamp(light.spotAngle + Input.GetAxis("Mouse ScrollWheel") * multiplier, minSpotAngle, maxSpotAngle);
    }

    void SetUI()
    {
        if (tmpBattery != null)
            tmpBattery.text = $"{batteryValue:N0}";
    }

    public void AddBattery(float value)
    {
        batteryValue = Mathf.Clamp(batteryValue + value, 0, 100);
    }
}
