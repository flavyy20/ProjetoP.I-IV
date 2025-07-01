using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class Personagem : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] float velMax = 10f;
    [SerializeField] float valorAcel = 10f;
    [SerializeField] float velRotacao = 8f;

    [Header("Pulo")]
    [SerializeField] float alturaPulo = 6f;
    [SerializeField] float gravidade = -15f;
    [SerializeField] float tempoPulo = 0.2f;

    [Header("Detecção de chão")]
    [SerializeField] float GroundedOffset = 0.22f;
    [SerializeField] float GroundedRadius = 0.68f;
    [SerializeField] LayerMask GroundLayers;

    CharacterController controller;
    Animator animator;

    Vector3 direcaoMovimento;
    float velocidadeY = 0f;
    float intervaloPulo = 0f;
    bool estaNoChao = false;
    bool estavaNoChao = false;
    float forcaVelocidade;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        velocidadeY = 0f;
        intervaloPulo = tempoPulo;
    }

    void Update()
    {
        ChecarChao();
        Mover();
        Pular();
    }

    void ChecarChao()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        estaNoChao = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (estaNoChao && !estavaNoChao)
        {
            intervaloPulo = 0f;
        }

        // Corrige o "quique" ao tocar o chão
        if (estaNoChao && velocidadeY < 0f)
        {
            velocidadeY = 0f;
        }

        estavaNoChao = estaNoChao;
    }

    void Mover()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float targetSpeed = moveInput == Vector2.zero ? 0f : velMax;

        float currentSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        if (currentSpeed < targetSpeed - speedOffset || currentSpeed > targetSpeed + speedOffset)
        {
            forcaVelocidade = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * valorAcel);
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
        Vector3 movimentoFinal = direcaoMovimento + Vector3.up * velocidadeY;
        controller.Move(movimentoFinal * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("b_Run", moveInput != Vector2.zero);
        }
    }

    void Pular()
    {
        if (estaNoChao)
        {
            // Reset da velocidade vertical ao tocar o chão
            velocidadeY = 0f;

            // Somente pula se apertar botão e tempo estiver liberado
            if (Input.GetButtonDown("Jump") && intervaloPulo <= 0f)
            {
                velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
                intervaloPulo = tempoPulo;

                if (animator != null)
                    animator.SetBool("b_Jump", true);
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
            // Se está no ar, aplica gravidade
            float velocidadeTerminal = -30f;
            if (velocidadeY > velocidadeTerminal)
            {
                velocidadeY += gravidade * Time.deltaTime;
            }

            if (animator != null)
                animator.SetBool("b_Jump", true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);

        if (Application.isPlaying && estaNoChao)
            Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(spherePosition, GroundedRadius);
    }
}