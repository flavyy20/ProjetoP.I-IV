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

    [Header("Configura��o UI")]
    public GameObject painelPuzzle; // Arraste o painel UI aqui
    public RectTransform areaPuzzle; // �rea onde os tubos podem ser movidos
    public Button btnFecharPuzzle;

    [Header("Configura��o do Puzzle")]
    public List<Conector> conectores;
    public float distanciaEncaixe = 30f; // Em pixels agora
    public List<TuboUI> tubosUI; // Lista dos tubos UI

    void Awake()
    {
        instance = this;
        //btnFecharPuzzle.onClick.AddListener(FecharPuzzle);


        //if (painelPuzzle == null)
        //{
        //    painelPuzzle = GameObject.Find("PainelPuzzle");

        //    if (painelPuzzle == null)
        //        Debug.LogError("PainelPuzzle n�o encontrado na cena!");
        //}

        if (painelPuzzle == null)
        {
            var todosOsPain�is = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (var cg in todosOsPain�is)
            {
                
                if (cg.gameObject.name == "PainelPuzzle") 
                {
                    Debug.Log("entrou no if");
                    painelPuzzle = cg.gameObject;
                    break;
                }
            }

            if (painelPuzzle == null)
            {
                Debug.LogError("PainelPuzzle n�o foi encontrado, mesmo desativado.");
            }
        }

       

        if (btnFecharPuzzle == null && painelPuzzle != null)
        {
            btnFecharPuzzle = painelPuzzle.GetComponentInChildren<Button>();
        }
        if (painelPuzzle != null)
        {
            painelPuzzle.SetActive(false);
        }
        AbrirPuzzle();
        btnFecharPuzzle?.onClick.AddListener(FecharPuzzle);
    }

    public void AbrirPuzzle()
    {
        if (painelPuzzle == null)
        {
            Debug.LogError("Painel do puzzle n�o est� atribu�do no Inspector!");
            return;
        }

        // Ativa o painel e traz para frente
        painelPuzzle.SetActive(true);
        painelPuzzle.transform.SetAsLastSibling();

        // Garante que o painel est� vis�vel
        CanvasGroup canvasGroup = painelPuzzle.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        ResetarPuzzle();
       // painelPuzzle.SetActive(true);
        //ResetarPuzzle();
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
    //public Action onPuzzleComplete; // Evento para notificar conclus�o
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

