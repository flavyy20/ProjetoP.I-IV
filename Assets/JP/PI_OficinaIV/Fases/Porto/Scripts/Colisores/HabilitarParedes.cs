using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilitarParedes : MonoBehaviour
{
    public Control_PuloJogador jogador;
    private Collider paredeCollider;

    void Start()
    {
        paredeCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (jogador != null)
        {
            paredeCollider.enabled = jogador.EstaNoChao();
        }
    }
}
