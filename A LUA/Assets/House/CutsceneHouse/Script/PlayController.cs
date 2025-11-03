using UnityEngine;

public class PlayController : MonoBehaviour
{
   [Header("Referências de controle do Player")]
    public MonoBehaviour firstPersonController;

    public GameObject cameraRoot;

    private bool controlsEnabled = true;

    /// <summary>
    /// Ativa ou desativa o controle do player
    /// </summary>
    public void EnableControls(bool enable)
    {
        controlsEnabled = enable;

        if (firstPersonController != null)
            firstPersonController.enabled = enable;

        if (cameraRoot != null)
            cameraRoot.SetActive(enable);

        Debug.Log($"[PlayerControlManager_Minimal] Controles {(enable ? "ativados" : "desativados")}");
    }

    /// <summary>
    /// Retorna true se o controle está ativo
    /// </summary>
    public bool AreControlsEnabled()
    {
        return controlsEnabled;
    }
}