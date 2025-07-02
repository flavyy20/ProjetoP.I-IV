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
    public float valorMin = 0f;
    public float valorMax = 5f;
    public static bool puzzleCompleto = false;
    public Action onPuzzleComplete;
    public Action<Transform> onPuzzleStart; 
    private float valorAtual;
    private float valorAlvo;

    void Start()
    {
        GerarNovoAlvo();
        
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
            Debug.Log("completo?:"+puzzleCompleto);
        }
    }

    void GerarNovoAlvo()
    {
        valorAlvo = UnityEngine.Random.Range(valorMin, valorMax);
        textoValorAlvo.text = "Alvo: " + valorAlvo.ToString("F1") + " kPa";
    }
}
