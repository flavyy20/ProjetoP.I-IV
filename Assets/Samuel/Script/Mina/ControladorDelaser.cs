using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorDelaser : MonoBehaviour
{
    [Header("Configurações")]
    public Transform origemLaser;
    public float distanciaMaxima = 100f;
    public LayerMask camadaBarragem;
    public bool laserAtivo = false;
    public PainelDiagnostico painelDiagnostico; // Novo campo

    private LineRenderer linhaLaser;
    private Barragem barragemAtual;

    void Start()
    {
        linhaLaser = GetComponent<LineRenderer>();
        linhaLaser.positionCount = 2;
        linhaLaser.enabled = false;
    }

    void Update()
    {
        if (!laserAtivo) return;

        AtualizarLaser();
        VerificarBarragem();
    }

    void AtualizarLaser()
    {
        linhaLaser.SetPosition(0, origemLaser.position);

        Ray raio = new Ray(origemLaser.position, origemLaser.forward);
        if (Physics.Raycast(raio, out RaycastHit hit, distanciaMaxima, camadaBarragem))
        {
            linhaLaser.SetPosition(1, hit.point);
            barragemAtual = hit.collider.GetComponent<Barragem>();
        }
        else
        {
            linhaLaser.SetPosition(1, origemLaser.position + origemLaser.forward * distanciaMaxima);
            barragemAtual = null;
        }
    }

    void VerificarBarragem()
    {
        if (barragemAtual != null && Input.GetKeyDown(KeyCode.E))
        {
            painelDiagnostico.AbrirPainel(barragemAtual);
        }
    }

    public void AlternarLaser()
    {
        laserAtivo = !laserAtivo;
        linhaLaser.enabled = laserAtivo;

        if (!laserAtivo && painelDiagnostico.painelAtivo)
        {
            painelDiagnostico.FecharPainel();
        }
    }
}
