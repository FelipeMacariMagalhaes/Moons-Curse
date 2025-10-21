using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class KidFollow : MonoBehaviour
{
 [Header("Referências")]
    public Transform player;
    public Animator animator;
    public NavMeshAgent agent;
    public TextMeshProUGUI rescueText; // Texto do UI (arraste no Inspector)

    [Header("Configurações")]
    public float followDistance = 2.5f;   // distância mínima pra parar
    public float runDistance = 6f;        // distância onde começa a correr
    public float rescueRange = 3f;        // distância pra mostrar UI de salvar
    public float baseOffset = 0.15f;      // altura acima do chão
    public float stopSmooth = 3f;         // suavização de parada

    private bool isFollowing = false;
    private bool isRescued = false;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.baseOffset = baseOffset;
        agent.updatePosition = true;
        agent.updateRotation = true;

        if (rescueText != null)
            rescueText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRescued)
        {
            FollowPlayer();
        }
        else
        {
            CheckForRescue();
        }

        // Corrige pequenas variações verticais
        agent.baseOffset = Mathf.Lerp(agent.baseOffset, baseOffset, Time.deltaTime * 5f);
    }

    void CheckForRescue()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= rescueRange)
        {
            if (rescueText != null)
                rescueText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                isRescued = true;
                if (rescueText != null)
                    rescueText.gameObject.SetActive(false);
            }
        }
        else
        {
            if (rescueText != null)
                rescueText.gameObject.SetActive(false);
        }
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 targetPos = player.position;

        if (distance > followDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPos);

            // Verifica se player está correndo (Shift)
            bool playerRunning = Input.GetKey(KeyCode.LeftShift);

            // Escolhe animação
            if (distance > runDistance || playerRunning)
            {
                animator.ResetTrigger("Walking");
                animator.ResetTrigger("lookAroundTrigger");
                animator.SetTrigger("Running");
            }
            else
            {
                animator.ResetTrigger("Running");
                animator.ResetTrigger("lookAroundTrigger");
                animator.SetTrigger("Walking");
            }
        }
        else
        {
            agent.velocity = Vector3.Lerp(agent.velocity, Vector3.zero, Time.deltaTime * stopSmooth);
            agent.isStopped = true;

            // Idle / Look Around
            animator.ResetTrigger("Running");
            animator.ResetTrigger("Walking");
            animator.SetTrigger("lookAroundTrigger");

            // Olha pro player
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), Time.deltaTime * 5f);
        }
    }
}
