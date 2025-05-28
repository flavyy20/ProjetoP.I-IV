using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCRandomWalk : MonoBehaviour
{
    public Transform player;
    public float followDistance = 50f;
    public float moveInterval = 3f;
    public float randomWalkRadius = 30f;

    private NavMeshAgent agent;
    private float timer;
    private bool isCaught = false;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        timer = moveInterval;
    }

    void Update()
    {
        if (isCaught || !agent.isOnNavMesh)
            return;

        // 🔥 PRIORIDADE: verificar se está sobre solo de lava
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                CaughtByLava();
                return; // importante: parar aqui se caiu
            }
        }

        // Se o player estiver perto, segue
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < followDistance)
            {
                agent.SetDestination(player.position);
                return;
            }
        }

        // Caso contrário, anda aleatoriamente
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;
            MoveRandomly();
        }
    }

    void MoveRandomly()
    {
        if (isCaught)
            return;

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * randomWalkRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 30f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public void CaughtByLava()
    {
        if (isCaught)
            return;

        isCaught = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.enabled = false;
        }

        if (rend != null)
        {
            rend.material.color = Color.red;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
    public void StopMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        isCaught = true; // Garante que ele pare de tentar andar
    }
}
