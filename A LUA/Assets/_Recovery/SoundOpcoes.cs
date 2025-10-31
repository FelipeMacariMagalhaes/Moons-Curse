using UnityEngine;

public class VolumeByMovement : MonoBehaviour
{
    public AudioSource audioSource; // arraste o som no Inspector
    public float minVolume = 0.1f;  // volume mínimo
    public float maxVolume = 1f;    // volume máximo
    public float minZ = -10f;       // posição mais pra trás
    public float maxZ = 10f;        // posição mais pra frente

    void Update()
    {
        if (audioSource != null)
        {
            // pega a posição Z do objeto
            float zPos = transform.position.z;

            // limita o valor entre minZ e maxZ
            zPos = Mathf.Clamp(zPos, minZ, maxZ);

            // transforma posição em volume (mapeia o valor)
            float t = (zPos - minZ) / (maxZ - minZ);
            float newVolume = Mathf.Lerp(minVolume, maxVolume, t);

            // aplica o volume
            audioSource.volume = newVolume;
        }
    }
}

