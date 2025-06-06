using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FloorFallManager : MonoBehaviour
{
    public Transform player;
    public float followDistance = 50f;
    public float moveInterval = 3f;
    public float randomWalkRadius = 30f;

    private bool wasOnWalkable = false;
    private NavMeshAgent agent;
    private float timer;
    private bool isCaught = false;

    private Renderer rend;
    private Color originalColor;

    private Rigidbody rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        timer = moveInterval;
    }

    void Update()
    {
        if (isCaught || !agent.isOnNavMesh)
            return;

        bool standingNow = IsStandingOnWalkable();

        if (!wasOnWalkable && standingNow)
        {
            wasOnWalkable = true;
        }
        else if (wasOnWalkable && !standingNow)
        {
            CaughtByLava();
            return;
        }

        if (player != null && Vector3.Distance(transform.position, player.position) < followDistance)
        {
            agent.SetDestination(player.position);
            return;
        }

        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            timer = 0f;
            MoveRandomly();
        }
    }

    bool IsStandingOnWalkable()
    {
        Vector3 checkPos = transform.position + Vector3.down * 0.1f;
        float checkRadius = 50f;

        Collider[] hits = Physics.OverlapSphere(checkPos, checkRadius);
        foreach (var col in hits)
        {
            if (LayerMask.LayerToName(col.gameObject.layer) == "Walkable")
                return true;
        }
        return false;
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
            agent.ResetPath();
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (rend != null)
            rend.material.color = Color.red;

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(Vector3.down * 100f, ForceMode.Impulse);

        StartCoroutine(FreezeAfterDelay(rb, 1.5f));
    }

    private System.Collections.IEnumerator FreezeAfterDelay(Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
