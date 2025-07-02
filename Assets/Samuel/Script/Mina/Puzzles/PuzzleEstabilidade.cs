using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEstabilidade : MonoBehaviour
{
    [Header("Configura��es")]
    public int recursosDisponiveis = 5;
    public GameObject[] suportesPrefabs; // Arraste os prefabs aqui no Inspector  
    public Transform[] zonasCriticas;    // Arraste as zonas cr�ticas aqui  

    [Header("Materiais")]
    public Material materialZonaInstavel; // Material vermelho  
    public Material materialZonaSegura;  // Material verde  

    public Action onPuzzleComplete;

    void Start()
    {
        // Destaca todas as zonas como inst�veis no in�cio  
        foreach (Transform zona in zonasCriticas)
        {
            zona.GetComponent<Renderer>().material = materialZonaInstavel;
        }
    }

    // Chamado quando o jogador arrasta um suporte para uma zona  
    public void PosicionarSuporte(int tipoSuporte, Transform zonaAlvo)
    {
        if (recursosDisponiveis <= 0) return;

        // Instancia o suporte na posi��o da zona  
        GameObject suporte = Instantiate(
            suportesPrefabs[tipoSuporte],
            zonaAlvo.position,
            Quaternion.identity
        );

        recursosDisponiveis--;
        zonaAlvo.GetComponent<Renderer>().material = materialZonaSegura;

        // Verifica se todas as zonas est�o est�veis  
        if (TodasZonasEstaveis())
        {
            onPuzzleComplete?.Invoke();
        }
    }

    bool TodasZonasEstaveis()
    {
        foreach (Transform zona in zonasCriticas)
        {
            if (zona.GetComponent<Renderer>().material != materialZonaSegura)
                return false;
        }
        return true;
    }

}
