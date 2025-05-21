using UnityEngine;
using UnityEngine.AI;

public class NPCRandomWalk : MonoBehaviour
{
    public float moveInterval = 3f;
    private NavMeshAgent agent;
    private Renderer rend;
    private Color originalColor;

    private bool isCaught = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        InvokeRepeating(nameof(MoveRandomly), 0f, moveInterval);
    }

    void Update()
    {
        if (isCaught)
            return;

        // Verifica se o NPC est� sobre um ch�o que virou "lava"
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                CaughtByLava();
            }
        }
    }

    void MoveRandomly()
    {
        if (isCaught)
            return;

        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 10f, NavMesh.AllAreas))
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

        // Ativa gravidade real para cair com o ch�o
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Ativa a f�sica
            rb.useGravity = true;
        }

        CancelInvoke(nameof(MoveRandomly));
    }
}
