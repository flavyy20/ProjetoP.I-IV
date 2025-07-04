using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseController : MonoBehaviour
{
    public int vitimasSalvas = 0;
    private bool faseConcluida = false;

    // Chamar quando o jogador salvar uma vítima
    public void SalvarVitima()
    {
        if (!faseConcluida)
        {
            vitimasSalvas++;
            // Atualiza UI se necessário
        }
    }

    // Chamar quando o jogador completar a fase com sucesso
    public void CompletarFase()
    {
        if (!faseConcluida)
        {
            faseConcluida = true;
            //FindObjectOfType<GerenciarCenasECO>().FinalizarMapa(false, vitimasSalvas);
            // Mostrar tela de vitória
        }
    }

    // Chamar quando a cidade for perdida (game over)
    public void PerderCidade()
    {
        if (!faseConcluida)
        {
            faseConcluida = true;
            //FindObjectOfType<GerenciarCenasECO>().FinalizarMapa(true, vitimasSalvas);
            // Mostrar tela de game over
        }
    }
}
