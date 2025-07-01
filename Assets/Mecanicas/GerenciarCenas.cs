using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GerenciarCenas : MonoBehaviour
{
    int numeroCena;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinalizarMapa()
    {
        //SaveData dados = FindObjectOfType<ControlarJogo>()._save.CarregarSave() ?? new SaveData(4);
        //dados.fases[numeroCena]._progressao = ; -> DEFINIR FORMA DE SALVAR O VALOR DA BARRA DE PROGRESSAO
        //dados.fases[numeroCena]._finalizado = true;
        //FindObjectOfType<ControlarJogo>()._save.SalvarJogo(dados);
        //dados.fases[numeroCena]._faseAtual = false;
    }

    public void IniciarMapa() //Iniciar no start
    {
        //SaveData dados = FindObjectOfType<ControlarJogo>()._save.CarregarSave() ?? new SaveData(4);
        //dados.fases[numeroCena]._progressao = ; -> DEFINIR FORMA DE SALVAR O VALOR DA BARRA DE PROGRESSAO
        //dados.fases[numeroCena]._finalizado = false;
        //FindObjectOfType<ControlarJogo>()._save.SalvarJogo(dados);
        //dados.fases[numeroCena]._faseAtual = true;
    }
}
