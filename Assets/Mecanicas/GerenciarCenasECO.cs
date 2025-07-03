using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciarCenasECO : MonoBehaviour
{
    private int numeroCena;
    private float tempoInicio;

    void Start()
    {
        numeroCena = SceneManager.GetActiveScene().buildIndex;
        tempoInicio = Time.time;
        IniciarMapa();
    }

    public void FinalizarMapa(bool cidadePerdida, int vitimasSalvas)
    {
        float tempoDecorrido = Time.time - tempoInicio;
        var controlador = ControlarJogo.Instance;
        SaveDataECO dados = controlador._save.CarregarSave() ?? new SaveDataECO(3);

        dados.fases[numeroCena].finalizado = !cidadePerdida;
        dados.fases[numeroCena].cidadePerdida = cidadePerdida;
        dados.fases[numeroCena].vitimasSalvas = vitimasSalvas;
        dados.fases[numeroCena].tempoConclusao = tempoDecorrido;
        dados.fases[numeroCena].faseAtual = false;

        // Se não perdeu, marca a próxima fase como atual
        if (!cidadePerdida && numeroCena + 1 < dados.fases.Length)
        {
            dados.fases[numeroCena + 1].faseAtual = true;
        }

        controlador._save.SalvarJogo(dados);
    }

    private void IniciarMapa()
    {
        var controlador = ControlarJogo.Instance;
        SaveDataECO dados = controlador._save.CarregarSave();

        if (dados != null && numeroCena < dados.fases.Length)
        {
            dados.fases[numeroCena].faseAtual = true;
            controlador._save.SalvarJogo(dados);
        }
    }
}
