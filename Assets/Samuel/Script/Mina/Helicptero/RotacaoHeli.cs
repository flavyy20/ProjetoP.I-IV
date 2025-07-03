using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacaoHeli : MonoBehaviour
{
    [Header("Configura��es de Rota��o")]
    public float velocidadeRotacao = 500f; // Velocidade base de rota��o
    public bool variarVelocidade = true; // Se a velocidade deve variar levemente
    public float variacaoVelocidade = 50f; // Intensidade da varia��o

    private float velocidadeAtual;

    void Start()
    {
        // Define uma velocidade inicial
        velocidadeAtual = velocidadeRotacao;

        // Adiciona varia��o aleat�ria se ativado
        if (variarVelocidade)
        {
            velocidadeAtual += Random.Range(-variacaoVelocidade, variacaoVelocidade);
        }
    }

    void Update()
    {
        // Rotaciona o objeto no eixo local (Z para h�lices convencionais)
        transform.Rotate(Vector3.up * velocidadeAtual * Time.deltaTime);
    }
}
