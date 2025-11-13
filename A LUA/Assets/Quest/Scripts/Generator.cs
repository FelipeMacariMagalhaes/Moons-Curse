using UnityEngine;

public class Generator : MonoBehaviour
{
    public bool isFixed = false;
    public GeneratorUI generatorUI;
    public AudioSource failSound;
    public ParticleSystem sparkEffect;
    public GameObject pressEUI; // UI de "Aperte E"

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && !isFixed)
        {
            pressEUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                generatorUI.StartRepair(this);
                pressEUI.SetActive(false);
            }
        }
        else
        {
            pressEUI.SetActive(false);
        }
    }

    public void FixGenerator()
    {
        if (isFixed) return;

        isFixed = true;
        generatorUI.HideUI();
        QuestManager.Instance.AddGeneratorFixed();
        Debug.Log("Gerador consertado!");
    }

    public void TriggerFailEffect()
    {
        if (failSound) failSound.Play();
        if (sparkEffect) sparkEffect.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            pressEUI.SetActive(false);
            generatorUI.HideUI();
        }
    }
}
