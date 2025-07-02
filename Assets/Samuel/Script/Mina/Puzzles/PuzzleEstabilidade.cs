using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEstabilidade : MonoBehaviour
{
    [Header("Configurações")]
    public int recursosDisponiveis = 5;
    public GameObject[] suportesPrefabs; // Arraste os prefabs aqui no Inspector  
    public Transform[] zonasCriticas;    // Arraste as zonas críticas aqui  

    [Header("Materiais")]
    public Material materialZonaInstavel; // Material vermelho  
    public Material materialZonaSegura;  // Material verde  

    public Action onPuzzleComplete;

    void Start()
    {
        // Destaca todas as zonas como instáveis no início  
        foreach (Transform zona in zonasCriticas)
        {
            zona.GetComponent<Renderer>().material = materialZonaInstavel;
        }
    }

    // Chamado quando o jogador arrasta um suporte para uma zona  
    public void PosicionarSuporte(int tipoSuporte, Transform zonaAlvo)
    {
        if (recursosDisponiveis <= 0) return;

        // Instancia o suporte na posição da zona  
        GameObject suporte = Instantiate(
            suportesPrefabs[tipoSuporte],
            zonaAlvo.position,
            Quaternion.identity
        );

        recursosDisponiveis--;
        zonaAlvo.GetComponent<Renderer>().material = materialZonaSegura;

        // Verifica se todas as zonas estão estáveis  
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
