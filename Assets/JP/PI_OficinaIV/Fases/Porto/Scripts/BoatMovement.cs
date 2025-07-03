/*using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovement : MonoBehaviour
{
    NavMeshAgent agent;

    public float moveSpeed = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 moveDirection;

    public Transform moveTo;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        agent.SetDestination(moveTo.position + moveTo.forward * 10f);
        agent.isStopped = IsAgentStopped(agent);
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = moveDirection * moveSpeed;
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Vitima"))
        if (agent.isStopped)
        {
            if (other.transform.parent == null)
            {
                other.transform.SetParent(transform);
                other.transform.localPosition = Vector3.zero;
            }
        }
    }

    private bool IsAgentStopped(NavMeshAgent agent)
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}*/