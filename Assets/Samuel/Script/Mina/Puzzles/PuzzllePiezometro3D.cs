using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PuzzllePiezometro3D : MonoBehaviour
{
    [Header("Referências 3D")]
    public Transform dial3D; // Objeto 3D do dial (arraste no Inspector)
    public TMP_Text display3D; // Texto 3D do display (TextMeshPro)
    public ParticleSystem particulaSucesso; // Efeito visual ao completar

    [Header("Configurações")]
    public float minAngle = -90f;
    public float maxAngle = 90f;
    public float minValor = 0f;
    public float maxValor = 10f;
    public float sensibilidade = 2f;

    private float currentAngle = 0f;
    private float valorAlvo;
    private bool puzzleCompleto = false;

    void Start()
    {
        GerarNovoAlvo();
    }

    void OnMouseDrag()
    {
        if (puzzleCompleto) return;

        float movimentoX = Input.GetAxis("Mouse X") * sensibilidade;
        currentAngle = Mathf.Clamp(currentAngle + movimentoX, minAngle, maxAngle);
        dial3D.localRotation = Quaternion.Euler(0, currentAngle, 0);

        AtualizarDisplay();
        VerificarCompleto();
    }

    // *** Método adicionado para resolver o erro ***
    float GetCurrentValue()
    {
        float percentual = Mathf.InverseLerp(minAngle, maxAngle, currentAngle);
        return Mathf.Lerp(minValor, maxValor, percentual);
    }

    void AtualizarDisplay()
    {
        float valorAtual = GetCurrentValue();
        display3D.text = $"Atual: {valorAtual.ToString("F1")} kPa\nAlvo: {valorAlvo.ToString("F1")} kPa";
    }

    void VerificarCompleto()
    {
        if (Mathf.Abs(GetCurrentValue() - valorAlvo) < 0.2f)
        {
            puzzleCompleto = true;
            particulaSucesso.Play();
            display3D.color = Color.green;

            // Chamada corrigida:
            if (PainelDiagnostico.instance != null)
            {
                PainelDiagnostico.instance.RegistrarProblemaResolvido("Sensores de Pressão com Falha");
            }
        }
    }

    void GerarNovoAlvo()
    {
        valorAlvo = Random.Range(minValor, maxValor);
    }
}
