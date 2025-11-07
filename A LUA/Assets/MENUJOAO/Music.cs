using UnityEngine;

public class MusicaPersistente : MonoBehaviour
{
    private static MusicaPersistente instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // mantém o objeto ao trocar de cena
        }
        else
        {
            Destroy(gameObject); // evita duplicar a música
        }
    }
}
