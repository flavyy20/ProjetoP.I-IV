using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class Personagem : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] float velocidade = 5f;
    [SerializeField] float velocidadeRotacao = 10f;

    [Header("Pulo")]
    [SerializeField] float alturaPulo = 1.2f;
    [SerializeField] float gravidade = -15.0f;
    [SerializeField] float tempoPulo = 0.5f;

    [Header("Detecção de chão")]
    [SerializeField] float GroundedOffset = -0.14f;
    [SerializeField] float GroundedRadius = 0.28f;
    [SerializeField] LayerMask GroundLayers;

    CharacterController controller;
    Animator animator;

    Vector3 direcaoMovimento;
    float velocidadeY;
    float tempoParaPular;
    bool estaNoChao;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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

        if (estaNoChao && velocidadeY < 0f)
        {
            velocidadeY = -2f; // mantém o personagem no chão
        }
    }

    void Mover()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 direcao = new Vector3(inputX, 0f, inputZ).normalized;

        bool estaAndando = direcao.magnitude >= 0.1f;

        if (estaAndando)
        {
            float anguloAlvo = Mathf.Atan2(direcao.x, direcao.z) * Mathf.Rad2Deg;
            Quaternion rotacaoAlvo = Quaternion.Euler(0f, anguloAlvo, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.deltaTime);
        }

        direcaoMovimento = transform.forward * direcao.magnitude * velocidade;

        if (animator != null)
        {
            animator.SetBool("Correndo", estaAndando);
        }
    }

    void Pular()
    {
        if (estaNoChao)
        {
            if (Input.GetKeyDown(KeyCode.Space) && tempoParaPular <= 0f)
            {
                velocidadeY = Mathf.Sqrt(alturaPulo * -2f * gravidade);
                tempoParaPular = tempoPulo;
            }

            if (tempoParaPular > 0f)
                tempoParaPular -= Time.deltaTime;
        }

        // Aplicar gravidade
        velocidadeY += gravidade * Time.deltaTime;

        // Juntar movimento horizontal e vertical
        Vector3 movimento = direcaoMovimento + Vector3.up * velocidadeY;

        // Move o personagem com colisões
        controller.Move(movimento * Time.deltaTime);
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

//[Header("Player")]
//[SerializeField] float MoveSpeed = 2.0f;

//[Range(0.0f, 0.3f)]
//[SerializeField] float RotationSmoothTime = 0.12f;
//[SerializeField] float SpeedChangeRate = 10.0f;

//[Space(10)]
//[SerializeField] float JumpHeight = 1.2f;
//[SerializeField] float Gravity = -15.0f;

//[Space(10)]
//[SerializeField] float JumpTimeout = 0.50f;
//[SerializeField] float FallTimeout = 0.15f;

//[Header("Player Grounded")]
//static bool Grounded;
//[SerializeField] float GroundedOffset = -0.14f;
//[SerializeField] float GroundedRadius = 0.28f;

//[SerializeField] LayerMask GroundLayers;
//[SerializeField] Animator animator;

//float _speed;
//float _targetRotation;
//float _rotationVelocity;
//float _verticalVelocity;
//float _jumpTimeoutDelta;
//float _fallTimeoutDelta;
//float _terminalVelocity = 53.0f;

//bool controlAnimator;

//PlayerInput _playerInput;
//CharacterController _controller;
//StarterAssetsInputs _input;

//void Start()
//{
//    _controller = GetComponent<CharacterController>();
//    _playerInput = GetComponent<PlayerInput>();

//    _jumpTimeoutDelta = JumpTimeout;
//    _fallTimeoutDelta = FallTimeout;

//    controlAnimator = TryGetComponent(out animator);
//}

//// Update is called once per frame
//void Update()
//{
//    controlAnimator = TryGetComponent(out animator);
//    MoverPersonagem();
//    Pular();
//    ChecarChao();
//}

//void ChecarChao()
//{
//    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
//        transform.position.z);
//    Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
//        QueryTriggerInteraction.Ignore);
//}

//void MoverPersonagem()
//{
//    float targetSpeed = MoveSpeed;

//    if (_input.move == Vector2.zero) 
//        targetSpeed = 0.0f;

//    // a reference to the players current horizontal velocity
//    float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

//    float speedOffset = 0.1f;
//    float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

//    // accelerate or decelerate to target speed
//    if (currentHorizontalSpeed < targetSpeed - speedOffset ||
//        currentHorizontalSpeed > targetSpeed + speedOffset)
//    {
//        // creates curved result rather than a linear one giving a more organic speed change
//        // note T in Lerp is clamped, so we don't need to clamp our speed
//        _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
//            Time.deltaTime * SpeedChangeRate);

//        // round speed to 3 decimal places
//        _speed = Mathf.Round(_speed * 1000f) / 1000f;
//    }
//    else
//    {
//        _speed = targetSpeed;
//    }

//    Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

//    Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

//    // move the player
//    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
//                     new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
//}

//void Pular()
//{
//    if (Grounded)
//    {
//        // reset the fall timeout timer
//        _fallTimeoutDelta = FallTimeout;


//        // stop our velocity dropping infinitely when grounded
//        if (_verticalVelocity < 0.0f)
//        {
//            _verticalVelocity = -2f;
//        }

//        // Jump
//        if (_input.jump && _jumpTimeoutDelta <= 0.0f)
//        {
//            // the square root of H * -2 * G = how much velocity needed to reach desired height
//            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
//        }

//        // jump timeout
//        if (_jumpTimeoutDelta >= 0.0f)
//        {
//            _jumpTimeoutDelta -= Time.deltaTime;
//        }
//    }
//    else
//    {
//        // reset the jump timeout timer
//        _jumpTimeoutDelta = JumpTimeout;

//        // fall timeout
//        if (_fallTimeoutDelta >= 0.0f)
//        {
//            _fallTimeoutDelta -= Time.deltaTime;
//        }

//        // if we are not grounded, do not jump
//        _input.jump = false;
//    }

//    // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
//    if (_verticalVelocity < _terminalVelocity)
//    {
//        _verticalVelocity += Gravity * Time.deltaTime;
//    }
//}