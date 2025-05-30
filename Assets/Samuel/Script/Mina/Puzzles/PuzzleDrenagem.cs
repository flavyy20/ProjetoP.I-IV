using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleDrenagem : MonoBehaviour
{
    public UnityEvent OnPuzzleConcluido;

    [Header("Referências 3D")]
    public Transform pontoInicial;
    public Transform pontoDreno;
    public ParticleSystem aguaPS;
    public float tempoVerificacao = 0.5f;
    public LayerMask camadaConexao; // Defina no Inspector quais layers devem ser verificadas

    private List<Transform> conexoes = new List<Transform>();

    void Start()
    {
        // Encontra todos os pontos de conexão automaticamente
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Conexao"))
                conexoes.Add(child);
        }
    }

    public void Configurar(System.Action aoConcluir)
    {
        OnPuzzleConcluido = new UnityEvent();
        OnPuzzleConcluido.AddListener(() => aoConcluir?.Invoke());
        IniciarPuzzle();
    }

    private void IniciarPuzzle()
    {
        aguaPS.transform.position = pontoInicial.position;
        aguaPS.Play();
        InvokeRepeating("VerificarConexoes", 0, tempoVerificacao);
    }

    private void VerificarConexoes()
    {
        bool todasConectadas = true;

        foreach (Transform conexao in conexoes)
        {
            if (!Physics.Raycast(conexao.position, conexao.forward, 1f, camadaConexao))
            {
                todasConectadas = false;
                break;
            }
        }

        if (todasConectadas && conexoes.Count > 0)
        {
            aguaPS.Stop();
            OnPuzzleConcluido?.Invoke();
            CancelInvoke("VerificarConexoes");
        }
    }
}
