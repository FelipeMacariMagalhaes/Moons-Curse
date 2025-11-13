using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneratorUI : MonoBehaviour
{
    public GameObject uiPanel;
    public Slider progressBar;
    public RectTransform skillZone;
    public RectTransform skillMarker;
    public float repairSpeed = 0.2f;

    private Generator currentGenerator;
    private PlayerInteractionController playerInteraction;
    private bool repairing = false;
    private bool skillActive = false;
    private bool markerMovingRight = true;
    private float markerSpeed = 400f;
    private bool failPenalty = false;

    public void StartRepair(Generator generator)
    {
        if (repairing) return;

        currentGenerator = generator;
        uiPanel.SetActive(true);
        repairing = true;

        // 🔒 Travar player e abaixar
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerInteraction = player.GetComponent<PlayerInteractionController>();

        if (playerInteraction != null)
            playerInteraction.LockPlayer();

        StartCoroutine(RepairRoutine());
    }

    public void HideUI()
    {
        uiPanel.SetActive(false);
        repairing = false;
        skillActive = false;

        // 🔓 Liberar player
        if (playerInteraction != null)
            playerInteraction.UnlockPlayer();
    }

    IEnumerator RepairRoutine()
    {
        progressBar.value = Mathf.Clamp01(progressBar.value);

        while (progressBar.value < 1f && repairing)
        {
            if (!failPenalty)
                progressBar.value += repairSpeed * Time.deltaTime;

            if (!skillActive && Random.value < 0.002f)
                StartCoroutine(SkillCheckRoutine());

            yield return null;
        }

        if (progressBar.value >= 1f)
        {
            currentGenerator.FixGenerator();
        }
    }

    IEnumerator SkillCheckRoutine()
    {
        skillActive = true;
        skillMarker.anchoredPosition = new Vector2(-100, 0);
        markerMovingRight = true;

        while (skillActive)
        {
            float dir = markerMovingRight ? 1 : -1;
            skillMarker.anchoredPosition += new Vector2(dir * markerSpeed * Time.deltaTime, 0);

            if (skillMarker.anchoredPosition.x > 100)
                markerMovingRight = false;
            else if (skillMarker.anchoredPosition.x < -100)
                markerMovingRight = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                float diff = Mathf.Abs(skillMarker.anchoredPosition.x - skillZone.anchoredPosition.x);

                if (diff < 20)
                {
                    Debug.Log("✅ Skill Check SUCCESS!");
                    progressBar.value += 0.1f;
                }
                else
                {
                    Debug.Log("💥 Skill Check FAIL!");
                    StartCoroutine(FailPenalty());
                }

                skillActive = false;
            }

            yield return null;
        }
    }

    IEnumerator FailPenalty()
    {
        failPenalty = true;
        progressBar.value -= 0.25f;
        progressBar.value = Mathf.Clamp01(progressBar.value);
        currentGenerator.TriggerFailEffect();
        Debug.Log("💥 Falha! Progresso reduzido.");

        yield return new WaitForSeconds(2f);
        failPenalty = false;
    }
}
