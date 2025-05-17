using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCWander : MonoBehaviour
{
    public string areaObjectName = "WanderArea";
    public float waitTime = 0.2f;

    private Bounds areaBounds;
    private NavMeshAgent agent;
    private Vector3 targetPosition;
    private Vector3 lastSafePosition;
    private bool isFalling = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject area = GameObject.Find(areaObjectName);
        if (area != null && area.GetComponent<BoxCollider>() != null)
        {
            areaBounds = area.GetComponent<BoxCollider>().bounds;
        }

        lastSafePosition = transform.position;
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (true)
        {
            if (!isFalling)
                targetPosition = GetSafeRandomPoint();

            while (!isFalling && Vector3.Distance(transform.position, targetPosition) > 0.5f)
            {
                if (IsOverUnsafeGround(out Transform unsafeGround))
                {
                    Debug.Log($"NPC {name} caiu com o solo {unsafeGround.name}");
                    agent.enabled = false;
                    transform.SetParent(unsafeGround); // afunda junto
                    isFalling = true;
                    yield break;
                }

                if (IsUnsafeAhead() || !IsPathValid(targetPosition))
                {
                    targetPosition = GetSafeRandomPoint();
                    continue;
                }

                agent.SetDestination(targetPosition);
                lastSafePosition = transform.position;

                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    bool IsOverUnsafeGround(out Transform unsafeGround)
    {
        unsafeGround = null;
        Vector3 rayOrigin = transform.position + Vector3.up * 1.5f;
        float rayDistance = 5f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Unsafe"))
            {
                unsafeGround = hit.collider.transform;
                return true;
            }
        }

        return false;
    }

    bool IsUnsafeAhead()
    {
        Vector3 origin = transform.position + Vector3.up * 1f;
        float maxDistance = 2.5f;
        int raysCount = 9;
        float spreadAngle = 60f;

        for (int i = 0; i < raysCount; i++)
        {
            float angle = Mathf.Lerp(-spreadAngle / 2, spreadAngle / 2, i / (float)(raysCount - 1));
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(origin, dir, out RaycastHit hit, maxDistance))
            {
                if (hit.collider.CompareTag("Unsafe"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    Vector3 GetSafeRandomPoint()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(areaBounds.min.x, areaBounds.max.x),
                transform.position.y,
                Random.Range(areaBounds.min.z, areaBounds.max.z)
            );

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                int unsafeArea = NavMesh.GetAreaFromName("Jump");
                if ((hit.mask & (1 << unsafeArea)) != 0)
                    continue;

                // Verifica se ponto também não é "Unsafe" por Raycast
                if (Physics.Raycast(hit.position + Vector3.up * 5f, Vector3.down, out RaycastHit rayHit, 10f))
                {
                    if (!rayHit.collider.CompareTag("Unsafe"))
                        return hit.position;
                }
            }
        }

        return transform.position;
    }

    bool IsPathValid(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(target, path))
        {
            foreach (var corner in path.corners)
            {
                if (NavMesh.SamplePosition(corner, out NavMeshHit hit, 1f, NavMesh.AllAreas))
                {
                    int unsafeArea = NavMesh.GetAreaFromName("Jump");
                    if ((hit.mask & (1 << unsafeArea)) != 0)
                        return false;
                }
            }
            return path.status == NavMeshPathStatus.PathComplete;
        }
        return false;
    }
}
