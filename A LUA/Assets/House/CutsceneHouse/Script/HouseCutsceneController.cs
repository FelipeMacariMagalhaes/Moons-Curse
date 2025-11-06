using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.SceneManagement;
public class HouseCutsceneController : MonoBehaviour
{
 public static HouseCutsceneController Instance;

    [Header("Referências")]
    public CameraSwitch cameraSwitcher;
    public PlayableDirector cutscenePhone1;
    public PlayableDirector cutscenePhone2;
    public PlayableDirector cutsceneWeapons;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        cameraSwitcher.SetActiveCamera(cameraSwitcher.cmPlayer);
    }

    public void PlayPhone1Cutscene()
    {
        StartCoroutine(PlayCutsceneRoutine(cutscenePhone1, cameraSwitcher.cmPhone1));
    }

    public void PlayPhone2Cutscene()
    {
        StartCoroutine(PlayCutsceneRoutine(cutscenePhone2, cameraSwitcher.cmPhone2));
    }

    public void PlayWeaponsCutscene()
    {
        StartCoroutine(PlayCutsceneRoutine(cutsceneWeapons, cameraSwitcher.cmWeapons, true));
    }

    private IEnumerator PlayCutsceneRoutine(PlayableDirector director, CinemachineCamera cam, bool isLast = false)
    {
        cameraSwitcher.SetActiveCamera(cam);
        director.Play();

        yield return new WaitWhile(() => director.state == PlayState.Playing);

        if (isLast)
        {
            yield return StartCoroutine(FadeScreenHouse.Instance.PlayFinalSequence());
        }
        else
        {
            cameraSwitcher.SetActiveCamera(cameraSwitcher.cmPlayer);
        }
    }
}