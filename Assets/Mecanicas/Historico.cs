using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Historico : MonoBehaviour
{
    public GameObject historicoPanel;
    public Text[] textosFases; // Textos para cada fase no painel

    void Start()
    {
        historicoPanel.SetActive(false);
    }

    public void MostrarHistorico()
    {
        historicoPanel.SetActive(true);
        //AtualizarHistorico();
    }

    public void EsconderHistorico()
    {
        historicoPanel.SetActive(false);
    }

    /*private void AtualizarHistorico()
    {
        FaseECO[] historico = ControlarJogoECO.Instance.ObterHistoricoFases();

        if (historico != null)
        {
            for (int i = 0; i < textosFases.Length && i < historico.Length; i++)
            {
                string status;
                if (historico[i].finalizado)
                {
                    status = "Concluída";
                }
                else if (historico[i].cidadePerdida)
                {
                    status = "Cidade Perdida";
                }
                else
                {
                    status = "Não iniciada";
                }

                TimeSpan tempo = TimeSpan.FromSeconds(historico[i].tempoConclusao);
                string tempoFormatado = historico[i].tempoConclusao > 0 ?
                    string.Format("{0:D2}:{1:D2}", tempo.Minutes, tempo.Seconds) : "00:00";

                textosFases[i].text =
                    $"Cidade {i + 1}\n" +
                    $"Status: {status}\n" +
                    $"Vítimas salvas: {historico[i].vitimasSalvas}\n" +
                    $"Tempo: {tempoFormatado}";
            }
        }
    }*/
}
