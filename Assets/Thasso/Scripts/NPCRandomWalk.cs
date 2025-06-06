using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCRandomWalk : MonoBehaviour
{
    public Transform player;
    public float followDistance = 50f;
    public float moveInterval = 3f;
    public float randomWalkRadius = 30f;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.5f;

    private bool wasOnWalkable = false;
    private NavMeshAgent agent;
    private float timer;
    private bool isCaught = false;
    private float groundedTime = 0f;
    private float requiredGroundedDuration = 1.5f;

    private bool isInSafeZone = false;
    public Collider safeZoneCollider; //  público agora para você arrastar manualmente no Inspector

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rend = GetComponent<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        timer = moveInterval;
    }

    void Update()
    {
        CheckIfInSafeZone(); //  verifica constantemente se está na zona segura

        if (isCaught)
        {
            TryDetachFromPlayer();
            return;
        }

        if (!agent.isOnNavMesh)
            return;

        bool standingNow = IsStandingOnWalkable();
        Debug.Log($"[{gameObject.name}] isStandingOnWalkable: {standingNow}");

        if (!wasOnWalkable)
        {
            if (standingNow)
            {
                wasOnWalkable = true;
                Debug.Log($"[{gameObject.name}] Começou em chão Walkable.");
            }
        }
        else
        {
            if (!standingNow)
            {
                Debug.Log($"[{gameObject.name}] Perdeu contato com Walkable. Vai cair.");
                CaughtByLava();
                return;
            }
        }

        if (isInSafeZone)
        {
            // está seguro, apenas anda aleatoriamente dentro da zona
        }
        else if (player != null && Vector3.Distance(transform.position, player.position) < followDistance)
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

    void CheckIfInSafeZone() //  novo método de detecção
    {
        if (safeZoneCollider == null)
            return;

        Vector3 pos = transform.position;
        Vector3 zoneMin = safeZoneCollider.bounds.min;
        Vector3 zoneMax = safeZoneCollider.bounds.max;

        bool insideXZ = pos.x >= zoneMin.x && pos.x <= zoneMax.x &&
                        pos.z >= zoneMin.z && pos.z <= zoneMax.z;

        if (insideXZ && !isInSafeZone)
        {
            isInSafeZone = true;
            Debug.Log($"{gameObject.name} entrou na Zona Segura (2D XZ).");
        }
        else if (!insideXZ && isInSafeZone)
        {
            isInSafeZone = false;
            Debug.Log($"{gameObject.name} saiu da Zona Segura.");
        }
    }

    bool IsStandingOnWalkable()
    {
        Vector3 checkPos = transform.position + Vector3.down * 0.1f;
        float checkRadius = 50f;

        Collider[] hits = Physics.OverlapSphere(checkPos, checkRadius);
        foreach (var col in hits)
        {
            string layerName = LayerMask.LayerToName(col.gameObject.layer);
            Debug.Log($"[{gameObject.name}] Detectado collider '{col.gameObject.name}' na layer '{layerName}'");

            if (layerName == "Walkable")
                return true;
        }

        Debug.Log($"[{gameObject.name}] Nenhum chão Walkable detectado.");
        return false;
    }

    void MoveRandomly()
    {
        if (isCaught)
            return;

        Vector3 destination;

        if (isInSafeZone && safeZoneCollider != null)
        {
            Bounds bounds = safeZoneCollider.bounds;
            destination = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                transform.position.y,
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
        else
        {
            Vector3 randomDirection = Random.insideUnitSphere * randomWalkRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 30f, NavMesh.AllAreas))
            {
                destination = hit.position;
            }
            else
            {
                return;
            }
        }

        if (NavMesh.SamplePosition(destination, out NavMeshHit finalHit, 5f, NavMesh.AllAreas))
        {
            agent.SetDestination(finalHit.position);
        }
    }

    //  Removido OnTriggerEnter (não é mais necessário)

    public void CaughtByLava()
    {
        if (isCaught)
            return;

        isCaught = true;

        Debug.Log($"NPC {gameObject.name} foi pego pela lava!");

        agent.ResetPath();
        agent.isStopped = true;
        agent.enabled = false;

        if (rend != null)
            rend.material.color = Color.red;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);

        StartCoroutine(SinkAfterFall(rb, 3f, 90f));
    }

    private IEnumerator SinkAfterFall(Rigidbody rb, float delay, float extraDepth)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 2f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - new Vector3(0, extraDepth, 0);

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 checkPos = transform.position + Vector3.down * 0.1f;
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }

    public bool IsCaught()
    {
        return isCaught;
    }

    public void AttachToPlayer(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;

        Debug.Log($"{gameObject.name} foi preso ao player.");
    }

    public void TryDetachFromPlayer()
    {
        if (!isCaught || transform.parent == null)
            return;

        if (IsStandingOnWalkable())
        {
            groundedTime += Time.deltaTime;

            if (groundedTime >= requiredGroundedDuration)
            {
                transform.SetParent(null);
                GetComponent<Collider>().enabled = true;
                GetComponent<Rigidbody>().isKinematic = true;

                agent.enabled = true;
                agent.isStopped = false;

                isCaught = false;
                groundedTime = 0f;

                if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                    transform.position = hit.position;

                if (rend != null)
                    rend.material.color = originalColor;

                Debug.Log($"{gameObject.name} tocou o chão e foi solto do player.");
            }
        }
        else
        {
            groundedTime = 0f;
        }
    }
}
