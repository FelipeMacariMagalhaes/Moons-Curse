using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GeneratorUI : MonoBehaviour
{
     public GameObject pressEText;   
    public Transform player;
    public float distanceToShow = 3f;
    public bool generatorCompleted = false;

    public bool isCompleted = false;
    public GameObject uiPanel;              
    public TextMeshProUGUI progressText;    
    public TextMeshProUGUI skillText;      
    public Image flashRed;                

    private Generator generator;
    private FirstPersonMovement movement;

    private float progress = 0f;
    private bool repairing = false;
    private bool skillActive = false;

    public float repairSpeed = 8f;

    KeyCode[] skillButtons = 
    {
        KeyCode.Space,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.F,
        KeyCode.R
    };

    KeyCode currentButton;

    private void Start()
    {
        pressEText.SetActive(false);
    }

    private void Update()
    {
        if (generatorCompleted)
        {
            pressEText.SetActive(false);    
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= distanceToShow)
            pressEText.SetActive(true);
        else
            pressEText.SetActive(false);
    }

    public void StartRepair(Generator gen)
    {
        if (repairing) return;

        generator = gen;
        repairing = true;
        uiPanel.SetActive(true);

        // trava player
        movement = FindObjectOfType<FirstPersonMovement>();
        if (movement != null)
            movement.speedOverrides.Add(() => 0f);

        StartCoroutine(RepairRoutine());
    }

    public void HideUI()
    {
        repairing = false;
        skillActive = false;
        uiPanel.SetActive(false);

        // destrava o player
        if (movement != null && movement.speedOverrides.Count > 0)
            movement.speedOverrides.RemoveAt(movement.speedOverrides.Count - 1);
    }

    IEnumerator RepairRoutine()
    {
        progressText.text = $"{(int)progress}%";
        skillText.text = "";

        while (progress < 100 && repairing)
        {
            progress += repairSpeed * Time.deltaTime;
            progress = Mathf.Clamp(progress, 0, 100);

            progressText.text = $"{(int)progress}%";

            // chance pequena de skill check
            if (!skillActive && Random.value < 0.003f)
                StartCoroutine(SkillCheckRoutine());

            yield return null;
        }

        if (progress >= 100)
        {
            generator.CompleteGenerator();
            HideUI();
        }
    }

    IEnumerator SkillCheckRoutine()
    {
        skillActive = true;

        currentButton = skillButtons[Random.Range(0, skillButtons.Length)];
        skillText.text = $"APERTE [{currentButton}] !";

        float window = 0.7f;
        float t = 0;
        bool pressed = false;

        while (t < window)
        {
            if (Input.GetKeyDown(currentButton))
            {
                pressed = true;
                break;
            }
            t += Time.deltaTime;
            yield return null;
        }

        if (pressed)
        {
            skillText.text = "✔ +10%";
            progress += 10;
        }
        else
        {
            skillText.text = "❌ FALHOU -20%";
            progress -= 20;
            progress = Mathf.Clamp(progress, 0, 100);

            StartCoroutine(FlashRed());
            generator.FailEffect();
        }

        yield return new WaitForSeconds(0.5f);
        skillText.text = "";
        skillActive = false;
    }

    IEnumerator FlashRed()
    {
        flashRed.color = new Color(1, 0, 0, 0.6f);
        yield return new WaitForSeconds(0.15f);
        flashRed.color = new Color(1, 0, 0, 0f);
    }
     public void CompleteUI()
    {
        generatorCompleted = true;
        pressEText.SetActive(false);
    }
}
