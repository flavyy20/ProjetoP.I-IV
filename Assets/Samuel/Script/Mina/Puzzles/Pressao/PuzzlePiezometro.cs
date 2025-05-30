using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePiezometro : MonoBehaviour
{
    [Header("Referências")]
    public Text textoValorAtual;
    public Text textoValorAlvo;
    public RectTransform dial;
    public Transform pontoFocoCamera; // Novo: Posição para a câmera focar
    public float valorMin = 0f;
    public float valorMax = 5f;
    private bool puzzleCompleto = false;
    public Action onPuzzleComplete;
    public Action<Transform> onPuzzleStart; // Novo: Evento de início (com ponto de foco)
    private float valorAtual;
    private float valorAlvo;

    void Start()
    {
        GerarNovoAlvo();
        onPuzzleStart?.Invoke(pontoFocoCamera); // Notifica o início do puzzle
    }

    void Update()
    {
        if (puzzleCompleto) return; // Sai se já estiver completo

        valorAtual = Mathf.Lerp(valorMin, valorMax, (dial.eulerAngles.z % 360) / 360f);
        textoValorAtual.text = valorAtual.ToString("F1") + " kPa";

        if (Mathf.Abs(valorAtual - valorAlvo) < 0.1f)
        {
            puzzleCompleto = true; // Marca como completo
            textoValorAtual.color = Color.green;
            onPuzzleComplete?.Invoke(); // Chama o evento UMA vez
            Debug.Log("Evento onPuzzleComplete disparado!");
        }
    }

    void GerarNovoAlvo()
    {
        valorAlvo = UnityEngine.Random.Range(valorMin, valorMax);
        textoValorAlvo.text = "Alvo: " + valorAlvo.ToString("F1") + " kPa";
    }
}
