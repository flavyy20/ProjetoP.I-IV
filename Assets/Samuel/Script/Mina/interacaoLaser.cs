using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interacaoLaser : MonoBehaviour
{
    public KeyCode teclaAtivacao = KeyCode.L;
    public ControladorDelaser laser;
    public float distanciaAtivacao = 15f;
    public Animator animator; // ReferÍncia para o Animator

    void Update()
    {
        if (Input.GetKeyDown(teclaAtivacao))
        {
            float distancia = Vector3.Distance(transform.position, laser.transform.position);

            if (distancia <= distanciaAtivacao)
            {
                laser.AlternarLaser();
                if (animator != null)
                {
                    animator.SetTrigger("Interagir");
                }
            }
            else
            {
                Debug.LogWarning("Muito longe para ativar o laser!");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (laser != null)
        {
            Gizmos.DrawWireSphere(laser.transform.position, distanciaAtivacao);
        }
    }
}
