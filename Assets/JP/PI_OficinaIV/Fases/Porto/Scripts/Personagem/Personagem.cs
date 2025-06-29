using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class Personagem : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] float velMax = 6f;
    [SerializeField] float valorAcel = 10f;
    [SerializeField] float velRotacao = 10f;
    [SerializeField] float estadoRotacao;

    [Header("Pulo")]
    [SerializeField] float alturaPulo;
    [SerializeField] float gravidade;
    [SerializeField] float tempoPulo;

    [Header("Detecção de chão")]
    [SerializeField] float GroundedOffset;
    [SerializeField] float GroundedRadius;
    [SerializeField] LayerMask GroundLayers;

    CharacterController controller;
    Animator animator;

    Vector3 direcaoMovimento, velAtual;
    float velocidadeY;
    float intervaloPulo;
    bool estaNoChao;
    float forcaRotacao, forcaVelocidade;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        velocidadeY = 0;
    }

    void Update()
    {
        ChecarChao();
        Mover();
        Pular();
        Debug.Log(estaNoChao);
    }

    void ChecarChao()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        estaNoChao = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (estaNoChao && velocidadeY < 0f)
        {
            velocidadeY = -2f; // mantém o personagem no chão
        }
    }

    void Mover()
    {
        float targetSpeed = velMax;

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (moveInput == Vector2.zero) targetSpeed = 0f;

        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            forcaVelocidade = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * valorAcel);
            forcaVelocidade = Mathf.Round(forcaVelocidade * 1000f) / 1000f;
        }
        else
        {
            forcaVelocidade = targetSpeed;
        }

        Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (inputDirection != Vector3.zero)
        {
            Quaternion alvoRotacao = Quaternion.LookRotation(inputDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, alvoRotacao, velRotacao * Time.deltaTime);
        }

        direcaoMovimento = inputDirection * forcaVelocidade;

        // Aplica movimento horizontal + vertical (gravidade/pulo)
        Vector3 movimentoFinal = direcaoMovimento + Vector3.up * velocidadeY;
        controller.Move(movimentoFinal * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("b_Run", moveInput != Vector2.zero);
        }
    }
    //if (animator != null)
    //{
    //    animator.SetBool("Correndo", estaAndando);
    //}

    void Pular()
    {
        if (estaNoChao)
        {
            if (Input.GetButtonDown("Jump") && intervaloPulo <= 0f)
            {
                velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);

                if (animator != null)
                    animator.SetBool("b_Jump", true);

                intervaloPulo = tempoPulo;
            }

            else
            {
                if (animator != null)
                    animator.SetBool("b_Jump", false);
            }

            if (intervaloPulo > 0f)
                intervaloPulo -= Time.deltaTime;
        }

        else
        {
            if (animator != null)
                animator.SetBool("b_Jump", true);
        }

        // Aplica gravidade com velocidade terminal
        float velocidadeTerminal = -600f;
        if (velocidadeY > velocidadeTerminal)
        {
            velocidadeY += gravidade * Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Calcular a posição da esfera com base no offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);

        // Muda de cor se estiver no chão
        if (Application.isPlaying && estaNoChao)
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawWireSphere(spherePosition, GroundedRadius);
    }
}