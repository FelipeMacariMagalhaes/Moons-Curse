using UnityEngine;
using Unity.Cinemachine;
public class CameraSwitch : MonoBehaviour
{
    public CinemachineCamera cmPlayer;
    public CinemachineCamera cmPhone1;
    public CinemachineCamera cmPhone2;
    public CinemachineCamera cmWeapons;

    void Start()
    {
        SetActiveCamera(cmPlayer);
    }

    public void SetActiveCamera(CinemachineCamera target)
    {
        var allCams = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
        foreach (var cam in allCams)
        {
            cam.Priority.Value = (cam == target) ? 20 : 0;
        }

        Debug.Log($"[CameraSwitcher] Ativando {target.name}");
    }
}
