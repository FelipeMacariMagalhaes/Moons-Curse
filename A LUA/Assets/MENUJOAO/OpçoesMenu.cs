using UnityEngine;
using UnityEngine.UI;

public class OpcoesMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider sensibilidadeSlider;
    public AudioSource musica; // Arraste o som/música aqui no inspetor
    public Camera playerCamera; // Arraste a câmera do jogador aqui

    private float sensibilidade = 2f;

    void Start()
    {
        // Define valores iniciais
        if (musica != null)
            musica.volume = volumeSlider.value;

        sensibilidade = sensibilidadeSlider.value;
    }

    void Update()
    {
        // Atualiza volume conforme mexe no slider
        if (musica != null)
            musica.volume = volumeSlider.value;

        // Atualiza sensibilidade
        sensibilidade = sensibilidadeSlider.value;
    }

    public float GetSensibilidade()
    {
        return sensibilidade;
    }
}

