using UnityEngine;

public class CameraController : MonoBehaviour
{
    public OpcoesMenu opcoes; // arraste o script OpcoesMenu aqui
    public Transform playerBody;
    float rotX = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * opcoes.GetSensibilidade();
        float mouseY = Input.GetAxis("Mouse Y") * opcoes.GetSensibilidade();

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

