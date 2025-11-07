using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public OpcoesMenu opcoes; // arraste aqui o objeto "Opcoes" com o script OpcoesMenu
    public Transform playerBody; // arraste o objeto do corpo do jogador (Player)
    float rotX = 0f;

    void Update()
    {
        // Pega o valor atual da sensibilidade do menu
        float sens = opcoes.GetSensibilidade();

        // Lê o movimento do mouse
        float mouseX = Input.GetAxis("Mouse X") * sens;
        float mouseY = Input.GetAxis("Mouse Y") * sens;

        // Faz a câmera girar
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

