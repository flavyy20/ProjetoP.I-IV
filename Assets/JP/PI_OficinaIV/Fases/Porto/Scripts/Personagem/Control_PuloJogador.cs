using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_PuloJogador : MonoBehaviour
{
    private Collider jogadorCollider;
    int layerParede;

    [SerializeField] LayerMask paredeLayer;

    void Start()
    {
        jogadorCollider = GetComponent<Collider>();
        layerParede = LayerMask.NameToLayer("Parede");
    }

    public bool EstaNoChao()
    {
        return ThirdPersonController.Grounded; ; // Ou seu método de verificação de chão atual
    }
}

