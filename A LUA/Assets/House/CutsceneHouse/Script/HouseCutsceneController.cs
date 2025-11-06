using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;
using System.Collections;

public class HouseCutsceneController : MonoBehaviour
{
      public static HouseCutsceneController Instance;

    [Header("Câmeras Virtuais")]
    public CinemachineVirtualCamera cmPlayer;
    public CinemachineVirtualCamera cmPhone1;
    public CinemachineVirtualCamera cmPhone2;
    public CinemachineVirtualCamera cmWeapons;

    [Header("Timelines")]
    public PlayableDirector cutscenePhone1;
    public PlayableDirector cutscenePhone2;
    public PlayableDirector cutsceneWeapons;

    private void Awake() => Instance = this;

    void Start()
    {
        // começa no player
        AtivarCamera(cmPlayer);
        Invoke(nameof(PlayCutscene1), 1f);
    }

    void PlayCutscene1()
    {
        AtivarCamera(cmPhone1);
        cutscenePhone1.Play();
    }

    public void PlayCutscene2()
    {
        AtivarCamera(cmPhone2);
        cutscenePhone2.Play();
    }

    public void PlayCutscene3()
    {
        AtivarCamera(cmWeapons);
        cutsceneWeapons.Play();
    }

    public void VoltarParaPlayer()
    {
        AtivarCamera(cmPlayer);
    }

    // Gerencia prioridades — o Brain troca automaticamente
    void AtivarCamera(CinemachineVirtualCamera ativa)
    {
        CinemachineVirtualCamera[] todas = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var vcam in todas)
        {
            if (vcam == ativa)
                vcam.Priority = 20;
            else
                vcam.Priority = 0;
        }

        Debug.Log($"[Cutscene] Ativando câmera: {ativa.name}");
    }
}