using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Barco : MonoBehaviour
{
    private NavMeshAgent agente;
    private bool podeMover = true;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            podeMover = true;
        }
    }

    public void MoverBarco(Vector3 destino)
    {
        if (podeMover)
        {
            agente.SetDestination(destino);
            podeMover = false;
        }
    }
}
