using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacaoHeli : MonoBehaviour
{
    [Header("Configurações de Rotação")]
    public float velocidadeRotacao = 500f; // Velocidade base de rotação
    public bool variarVelocidade = true; // Se a velocidade deve variar levemente
    public float variacaoVelocidade = 50f; // Intensidade da variação

    private float velocidadeAtual;

    void Start()
    {
        // Define uma velocidade inicial
        velocidadeAtual = velocidadeRotacao;

        // Adiciona variação aleatória se ativado
        if (variarVelocidade)
        {
            velocidadeAtual += Random.Range(-variacaoVelocidade, variacaoVelocidade);
        }
    }

    void Update()
    {
        // Rotaciona o objeto no eixo local (Z para hélices convencionais)
        transform.Rotate(Vector3.up * velocidadeAtual * Time.deltaTime);
    }
}
