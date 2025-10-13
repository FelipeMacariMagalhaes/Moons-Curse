using UnityEngine;
using TMPro;
public class LanternaController : MonoBehaviour
{
    private Light light;
    public AudioSource Som;
    
    public TextMeshProUGUI tmpBattery;
    public float minSpotAngle = 5;
    public float maxSpotAngle = 70;
    public float multiplier = 5;

    public float multiplierReduceBattery = 10;

    private float batteryValue = 100;
    
    void Start()
    {
        light = GetComponentInChildren<Light>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
           TurnFlashLight();

        if(light.enabled)
        {
            Focar();
            Reduce();
        }   
    }
    void TurnFlashLight()
    {
        if(batteryValue != 0)
             light.enabled = !light.enabled;
        else
            light.enabled = false;
            
        

           Som.Play();
    }
    void Reduce()
    {
        batteryValue = Mathf.Clamp(batteryValue -= multiplierReduceBattery * Time.deltaTime, 0, 100); 
    }
    void Focar()
    {
        light.spotAngle = Mathf.Clamp(light.spotAngle += Input.GetAxis("Mouse ScrollWheel") * multiplier, minSpotAngle, maxSpotAngle);
    }
    void SetUI()
    {
        tmpBattery.text = batteryValue.ToString("N0");
    }
    
    public void AddBattery(float value)
    {
        batteryValue = Mathf.Clamp(batteryValue += value, 0, 100);
    }
    
}