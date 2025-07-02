using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{

    public static PuzzleManager instance;
    public Action onPuzzleComplete;
    public static bool puzzleDrenagemCompleto = false;

    [Header("Configuração UI")]
    public GameObject painelPuzzle; // Arraste o painel UI aqui
    public RectTransform areaPuzzle; // Área onde os tubos podem ser movidos
    public Button btnFecharPuzzle;

    [Header("Configuração do Puzzle")]
    public List<Conector> conectores;
    public float distanciaEncaixe = 30f; // Em pixels agora
    public List<TuboUI> tubosUI; // Lista dos tubos UI

    void Awake()
    {
        instance = this;
        btnFecharPuzzle.onClick.AddListener(FecharPuzzle);
    }

    public void AbrirPuzzle()
    {
        painelPuzzle.SetActive(true);
        ResetarPuzzle();
    }

    public void FecharPuzzle()
    {
        painelPuzzle.SetActive(false);
    }

    public void VerificarCompleto()
    {
        foreach (Conector conector in conectores)
        {
            if (!conector.estaOcupado) return;
        }

        puzzleDrenagemCompleto = true;
        Debug.Log("PUZZLE COMPLETO!");
        onPuzzleComplete?.Invoke();
        FecharPuzzle();
    }

    void ResetarPuzzle()
    {
        foreach (TuboUI tubo in tubosUI)
        {
            tubo.ResetarPosicao();
        }

        foreach (Conector conector in conectores)
        {
            conector.estaOcupado = false;
        }
    }

    //public static PuzzleManager instance;
    //public Action onPuzzleComplete; // Evento para notificar conclusão
    //public static bool puzzleDrenagemCompleto =false;

    //[Header("Config")]
    //public List<Conector> conectores;
    //public float distanciaEncaixe = 0.5f;

    //void Awake() => instance = this;

    //public void VerificarCompleto()
    //{
    //    foreach (Conector conector in conectores)
    //    {
    //        if (!conector.estaOcupado) return;
    //    }
    //    puzzleDrenagemCompleto = true;

    //    Debug.Log("PUZZLE COMPLETO!");
    //    Debug.Log("completo?:" + puzzleDrenagemCompleto);
    //    onPuzzleComplete?.Invoke(); // Dispara o evento
    //}
}

