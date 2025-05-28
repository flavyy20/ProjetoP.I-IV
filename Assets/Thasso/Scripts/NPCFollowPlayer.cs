using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCFollowPlayer : MonoBehaviour
{
    public Transform player; // Refer�ncia ao jogador
    public float followDistance = 5f; // Dist�ncia para come�ar a seguir

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!agent.isOnNavMesh)
            Debug.LogError($"{name} n�o est� sobre o NavMesh!");
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < followDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
