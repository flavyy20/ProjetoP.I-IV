using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoPerso : MonoBehaviour
{
    private Animator animator;
    private Camera cam;

    private float velocidadeVertical = 0f;
    private Vector3 movimentoHorizontal;

    public float velocidade = 5f, alturaPulo = 7f, gravidade = 20f;

    public LayerMask layerChao, layerVitima;
    public Transform checadorChao;
    public float raioChao = 0.3f;
    public BoatMovement bote;

    public bool correndo, noChao, pertoVitima;

    void Awake()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        Animacao();
        VerificarChao();
        CalcularMovimento();
        AplicarGravidadeEPulo();
        MoverPersonagem();

        if (pertoVitima && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerVitima))
            {
                Vector3 position = hit.collider.transform.position;
                position.y = 0f;
                bote.SetDestination(position);
            }
        }
    }

    private void Animacao()
    {
        if (!noChao) animator.Play("jump");
        else if (correndo) animator.Play("run");
        else animator.Play("idle");
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

            Quaternion targetRotation = Quaternion.LookRotation(movimentoHorizontal, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

            if (!correndo) correndo = true;
        }
        else
        {
            if (correndo) correndo = false;
        }
    }

    private void AplicarGravidadeEPulo()
    {
        if (noChao)
        {
            if (velocidadeVertical < 0f) velocidadeVertical = 0f;

            if (Input.GetKeyDown(KeyCode.Space)) velocidadeVertical = alturaPulo;
        }
        else velocidadeVertical -= gravidade * Time.deltaTime;
    }

    private void MoverPersonagem()
    {
        Vector3 movimentoFinal = new(movimentoHorizontal.x, velocidadeVertical, movimentoHorizontal.z);
        transform.position += movimentoFinal * Time.deltaTime;
    }

    private void VerificarChao() => noChao = Physics.CheckSphere(checadorChao.position, raioChao, layerChao);

    void OnDrawGizmosSelected()
    {
        if (checadorChao != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checadorChao.position, raioChao);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vitima"))
        {
            pertoVitima = true;
            other.GetComponent<Outline>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Vitima"))
        {
            pertoVitima = false;
            other.GetComponent<Outline>().enabled = false;
        }
    }
}
