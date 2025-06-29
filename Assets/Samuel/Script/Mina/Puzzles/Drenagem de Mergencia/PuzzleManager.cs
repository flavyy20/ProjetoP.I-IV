using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    public static PuzzleManager instance;
    public Action onPuzzleComplete; // Evento para notificar conclusão
    public static bool puzzleDrenagemCompleto =false;

    [Header("Config")]
    public List<Conector> conectores;
    public float distanciaEncaixe = 0.5f;

    void Awake() => instance = this;

    public void VerificarCompleto()
    {
        foreach (Conector conector in conectores)
        {
            if (!conector.estaOcupado) return;
        }
        puzzleDrenagemCompleto = true;

        Debug.Log("PUZZLE COMPLETO!");
        Debug.Log("completo?:" + puzzleDrenagemCompleto);
        onPuzzleComplete?.Invoke(); // Dispara o evento
    }
}

