using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioSource audioSource; // arraste o Audio Source no Inspector

    void Update()
    {
        // Se o jogador clicar com o botão esquerdo do mouse
        if (Input.GetMouseButtonDown(0))
        {
            if (audioSource != null)
            {
                // Toca o som uma vez
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}

