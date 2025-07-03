using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoPerso : MonoBehaviour
{
    public float velocidade = 5f;
    public float alturaPulo = 7f;
    public float gravidade = 20f;

    public LayerMask layerChao;
    public Transform checadorChao;
    public float raioChao = 0.3f;

    private Animator animator;
    private Camera cam;
    public bool correndo;
    public bool noChao;

    private float velocidadeVertical = 0f;
    private Vector3 movimentoHorizontal;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        VerificarChao();
        CalcularMovimento();
        AplicarGravidadeEPulo();
        MoverPersonagem();
    }

    private void CalcularMovimento()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(inputX, 0f, inputZ).normalized;
        movimentoHorizontal = Vector3.zero;

        if (inputDirection.magnitude > 0f)
        {
            Vector3 cameraForward = cam.transform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = cam.transform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            movimentoHorizontal = (cameraForward * inputZ + cameraRight * inputX).normalized * velocidade;

            // Rotaciona o personagem
            Quaternion targetRotation = Quaternion.LookRotation(movimentoHorizontal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

            if (!correndo)
            {
                correndo = true;
                animator.Play("run");
            }
        }
        else
        {
            if (correndo)
            {
                correndo = false;
                animator.Play("idle");
            }
        }
    }

    private void AplicarGravidadeEPulo()
    {
        if (noChao)
        {
            // Impede que a gravidade continue aplicando força pra baixo
            if (velocidadeVertical < 0f)
            {
                velocidadeVertical = 0f; // valor negativo leve para manter contato com o chão
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocidadeVertical = alturaPulo;
                animator.Play("jump"); // troque para sua animação
            }
        }
        else
        {
            velocidadeVertical -= gravidade * Time.deltaTime;
        }
    }

    private void MoverPersonagem()
    {
        Vector3 movimentoFinal = new Vector3(movimentoHorizontal.x, velocidadeVertical, movimentoHorizontal.z);
        transform.position += movimentoFinal * Time.deltaTime;
    }

    private void VerificarChao()
    {
        // Confere se há colisão com o chão
        noChao = Physics.CheckSphere(checadorChao.position, raioChao, layerChao);
    }

    void OnDrawGizmosSelected()
    {
        if (checadorChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checadorChao.position, raioChao);
        }
    }
}
