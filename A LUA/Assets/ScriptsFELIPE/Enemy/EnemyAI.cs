using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Som de Pulso")]
    public AudioSource pulseAudio;
    public float pulseMaxVolume = 1f;   
    public float pulseMinVolume = 0.2f;
    public float pulseSpeed = 6f; // velocidade do pulso

    [Header("Patrulha")]
    public float minDistanceToPoint = 1f;
    public float idleTime = 3f;
    public float speedOfNavegation = 6f;
    public GameObject[] navegationsPoints;

    [Header("Follow Player")]
    public float speedOfFollow = 6f;
    public Transform eyes;
    public float visionRadius = 10f;
    public float visionAngle = 45f;

    [Header("Vinheta / Game Over")]
    public Image screenFade;
    public float fadeSpeed = 0.5f;
    public float fadeMaxDistance = 10f;
    public float gameOverTime = 5f;
    private bool gameOverTriggered = false;
    private float fadeTimer = 0f;
    public float maxDistance = 10f;

    [Header("Sons")]
    public AudioSource sinisterAudio;
    public float maxSinisterVolume = 1f;
    public float minSinisterVolume = 0f;
    public float sinisterMaxDistance = 15f;
    public AudioSource alertAudio;
    public float alertCooldown = 3600f;
    private float lastAlertTime = -9999f;

    private NavMeshAgent navMesh;
    private Animator anim;
    private Transform target;
    private int pointIndex;
    private bool isIdling = false;
    private bool canSeePlayer = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        navegationsPoints = GameObject.FindGameObjectsWithTag("NavegationsPoint");
        pointIndex = GetRandomPointIndex();

        if (sinisterAudio != null)
        {
            sinisterAudio.loop = true;
            sinisterAudio.Play();
        }
    }

    void Update()
    {
        if (gameOverTriggered) return;

    canSeePlayer = CanSeePlayer();

    UpdateSinisterVolume();
    CheckAlertSound();
    UpdateScreenFade();

        if (canSeePlayer)
        {
            // Persegue o player
            if (isIdling) StopAllCoroutines();
            isIdling = false;

            navMesh.isStopped = false;
            navMesh.speed = speedOfFollow;
            navMesh.SetDestination(target.position);

            anim.SetBool("Follow", true);
            anim.SetBool("Navegation", false);
        }
            else
        {
            // Se estava perseguindo, reinicie a patrulha
            anim.SetBool("Follow", false);

            // Se não está idling e não está indo para o ponto, comece a patrulha
            if (!isIdling && !navMesh.pathPending && navMesh.remainingDistance < 0.1f)
            Navegation();
        }
    }


    private bool CanSeePlayer()
    {
        if (target == null) return false;
        Vector3 dir = (target.position - eyes.position).normalized;
        if (Vector3.Angle(eyes.forward, dir) < visionAngle / 2f)
        {
            if (Physics.Raycast(eyes.position, dir, out RaycastHit hit, visionRadius))
            {
                if (hit.collider.CompareTag("Player"))
                    return true;
            }
        }
        return false;
    }

    private void Navegation()
    {
        if (navegationsPoints.Length == 0) return;

        if (Vector3.Distance(transform.position, navegationsPoints[pointIndex].transform.position) > minDistanceToPoint)
        {
            anim.SetBool("Navegation", true);
            navMesh.isStopped = false;
            navMesh.speed = speedOfNavegation;
            navMesh.SetDestination(navegationsPoints[pointIndex].transform.position);
        }
        else
        {
            StartCoroutine(IdleAtPoint());
        }
    }

    private IEnumerator IdleAtPoint()
    {
        isIdling = true;
        anim.SetBool("Navegation", false);
        navMesh.isStopped = true;
        yield return new WaitForSeconds(idleTime);
        pointIndex = GetRandomPointIndex();
        anim.SetBool("Navegation", true);
        navMesh.isStopped = false;
        navMesh.SetDestination(navegationsPoints[pointIndex].transform.position);
        isIdling = false;
    }

    private int GetRandomPointIndex()
    {
        if (navegationsPoints == null || navegationsPoints.Length == 0) return 0;
        if (navegationsPoints.Length == 1) return 0;
        int i = Random.Range(0, navegationsPoints.Length);
        if (i == pointIndex) i = (i + 1) % navegationsPoints.Length;
        return i;
    }

    private void UpdateSinisterVolume()
    {
        if (target == null || sinisterAudio == null) return;
        float distance = Vector3.Distance(transform.position, target.position);
        float volume = Mathf.Lerp(maxSinisterVolume, minSinisterVolume, distance / sinisterMaxDistance);
        sinisterAudio.volume = Mathf.Clamp(volume, minSinisterVolume, maxSinisterVolume);
    }

    private void CheckAlertSound()
    {
        if (target == null || alertAudio == null) return;
        if (canSeePlayer && Time.time - lastAlertTime >= alertCooldown)
        {
            alertAudio.Play();
            lastAlertTime = Time.time;
        }
    }

    private void UpdateScreenFade()
    {
        if (screenFade == null || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        float t = Mathf.Clamp01(distance / maxDistance);

        float cutoffBase = Mathf.Lerp(1f, 0f, 1f - t);

        cutoffBase = Mathf.SmoothStep(1f, 0f, 1f - t);

        float pulse = 0f;

        if (cutoffBase < 0.4f)
        {
            pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.05f * (1f - cutoffBase * 2f);

            // Som pulsante
            if (pulseAudio != null && !pulseAudio.isPlaying)
            pulseAudio.Play();

            if (pulseAudio != null)
            {
                // Volume proporcional à proximidade
                float vol = Mathf.Lerp(pulseMinVolume, pulseMaxVolume, 1f - cutoffBase);
                pulseAudio.volume = vol;
            }
        }
        else
        {
            if (pulseAudio != null && pulseAudio.isPlaying)
                pulseAudio.Stop();
        }

        float finalCutoff = Mathf.Clamp01(cutoffBase + pulse);

        // Aplica no material
        Material mat = screenFade.material;
            if (mat != null && mat.HasProperty("_Cutoff"))
                mat.SetFloat("_Cutoff", finalCutoff);

        // Game Over quando a tela fecha completamente
            if (finalCutoff <= 0.05f && !gameOverTriggered)
        {
        gameOverTriggered = true;
        StartCoroutine(GameOverDelay());
        }
    }

    private IEnumerator GameOverDelay()
    {
    yield return new WaitForSeconds(1.5f);
    SceneManager.LoadScene("0");
    }
}