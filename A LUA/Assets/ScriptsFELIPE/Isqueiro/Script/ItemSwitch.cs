using UnityEngine;

public class ItemSwitch : MonoBehaviour
{
   public GameObject flashlight;
    public GameObject lighter;
    public GameObject cigarette;

    public KeyCode switchKey = KeyCode.Q;
    public KeyCode smokeKey = KeyCode.R;

    public LightIsqueiro lighterScript;

    private int currentItem = 0; // 0 = lanterna, 1 = isqueiro
    private bool hasLighter = false;

    void Start()
    {
        SetActiveItem(currentItem);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            SwitchItem();
        }

        if (Input.GetKeyDown(smokeKey))
        {
            TrySmoke();
        }
    }

    public void GiveLighter()
    {
        hasLighter = true; // só marca que o player pegou o isqueiro
    }

    void SwitchItem()
    {
        currentItem++;
        if (currentItem > 1) currentItem = 0;

        SetActiveItem(currentItem);
    }

    void SetActiveItem(int index)
    {
        if (flashlight != null)
            flashlight.SetActive(index == 0);

        if (lighter != null)
            lighter.SetActive(index == 1 && hasLighter); // só ativa o isqueiro se foi pego e selecionado

        if (cigarette != null)
            cigarette.SetActive(false);

        if (lighterScript != null)
            lighterScript.TurnOff(); // garante que a luz desligue ao trocar
    }

    void TrySmoke()
    {
        if (!hasLighter) return;
        if (lighterScript == null || !lighterScript.IsLit()) return;
        if (cigarette != null)
            cigarette.SetActive(true);
    }
}

