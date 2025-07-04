using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovement : MonoBehaviour
{
    NavMeshAgent agent;

    public float moveSpeed = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 moveDirection, _base;

    public Transform moveTo;

    bool comVitima;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _base = transform.position;
    }

    private void Update()
    {
        agent.isStopped = IsAgentStopped(agent);

        if (comVitima && agent.isStopped)
        {
            if (Vector3.Distance(transform.position, _base) < agent.stoppingDistance)
            {
                for (int i = 0; i < transform.childCount; i++)
                    if (transform.GetChild(i).CompareTag("Vitima"))
                        Destroy(transform.GetChild(i).gameObject);
                comVitima = false;
            }
        }
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
                    comVitima = true;
                }
                agent.SetDestination(_base);
            }
    }

    public void SetDestination(Vector3 position)
    {
        if (agent.isStopped) agent.SetDestination(position);
    }

    private bool IsAgentStopped(NavMeshAgent agent)
    {
        return !agent.pathPending &&
               agent.remainingDistance <= agent.stoppingDistance &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}