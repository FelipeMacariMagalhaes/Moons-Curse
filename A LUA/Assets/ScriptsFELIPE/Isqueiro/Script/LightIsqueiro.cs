using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightIsqueiro : MonoBehaviour
{
   public Light flameLight;
    public AudioSource audioSource;
    public Slider fuelBar;

    public AudioClip fireLoop;
    public AudioClip clickFail;
    public AudioClip outOfFuel;

    public float flickerIntensity = 0.3f;
    public float shakeIntensity = 0.05f;
    public float shakeDuration = 0.2f;
    public int maxUses = 15;
    public KeyCode useKey = KeyCode.F;

    private bool isOn = false;
    private bool outOfGas = false;
    private float currentFuel;
    private Vector3 originalPos;

    public float fuelConsumeSpeed = 1f; // velocidade de consumo por segundo quando ligado

    void Start()
    {
        currentFuel = maxUses;
        originalPos = transform.localPosition;
        if (flameLight != null) flameLight.enabled = false;
        if (fuelBar != null)
        {
            fuelBar.maxValue = maxUses;
            fuelBar.value = currentFuel;
        }
    }

    void OnEnable()
    {
        if (!outOfGas)
        {
            isOn = false;
            if (flameLight != null) flameLight.enabled = false;
            if (audioSource != null)
            {
                audioSource.Stop();
                audioSource.loop = false;
            }
        }
    }

    void OnDisable()
    {
        TurnOff();
    }

    void Update()
    {
        if (Input.GetKeyDown(useKey))
        {
            ToggleLighter();
        }

        if (isOn)
        {
            if (flameLight != null)
                flameLight.intensity = 1 + Mathf.Sin(Time.time * 20f) * flickerIntensity;

            currentFuel -= Time.deltaTime * fuelConsumeSpeed;
            if (fuelBar != null)
                fuelBar.value = Mathf.Clamp(currentFuel, 0, maxUses);

            if (currentFuel <= 0 && !outOfGas)
            {
                outOfGas = true;
                TurnOff();
                if (audioSource != null && outOfFuel != null)
                    audioSource.PlayOneShot(outOfFuel);
            }
        }
    }

    void ToggleLighter()
    {
        if (outOfGas) return;
        if (!isOn) TurnOn();
        else TurnOff();
    }

    void TurnOn()
    {
        isOn = true;
        if (flameLight != null) flameLight.enabled = true;
        if (audioSource != null && fireLoop != null)
        {
            audioSource.loop = true;
            audioSource.clip = fireLoop;
            audioSource.Play();
        }
    }

    public void TurnOff()
    {
        isOn = false;
        if (flameLight != null) flameLight.enabled = false;
        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
    }

    public bool IsLit()
    {
        return isOn;
    }
}