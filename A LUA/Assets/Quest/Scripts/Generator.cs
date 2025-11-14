using UnityEngine;

public class Generator : MonoBehaviour
{
    public GeneratorUI ui;            // PRECISA ARRSTAR AQUI
    public ParticleSystem sparkEffect;
    public AudioSource failSound;

    private bool playerInside = false;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            ui.StartRepair(this);
        }
    }

    public void CompleteGenerator()
    {
        ui.HideUI();    

         
        if (QuestManager.Instance != null)
            QuestManager.Instance.AddGeneratorCompleted();
        else
            Debug.LogError("❌ QuestManager.Instance é NULL! Adicione ele na cena.");

        ui.CompleteUI();
    }

    public void FailEffect()
    {
        if (sparkEffect) sparkEffect.Play();
        if (failSound) failSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            ui.HideUI();
        }
    }
}
