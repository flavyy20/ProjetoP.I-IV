using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidadeMovimento = 30f;
    public Transform cameraPrincipal;
    public Animator animator; 
    public float velocidadeRotacao= 8f;
    Rigidbody rb;
    private float rotacaoX = 0f;
    private bool estaCorrendo = false;
    private Vector3 direcaoMovimento;
    public float forcaQueda = 20f; 
    public float distanciaVerificacaoChao =10f; 


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        estaCorrendo = (horizontal != 0f || vertical != 0f);

        direcaoMovimento = new Vector3(horizontal, 0f, vertical).normalized;

       



        if (animator != null)
        {
            animator.SetBool("Correndo", estaCorrendo);
        }

    }

    

    void FixedUpdate()
    {
        if (direcaoMovimento.magnitude >= 0.1f)
        {
            // Rotação suave
            float anguloAlvo = Mathf.Atan2(direcaoMovimento.x, direcaoMovimento.z) * Mathf.Rad2Deg;
            Quaternion rotacaoAlvo = Quaternion.Euler(0f, anguloAlvo, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.deltaTime);

            // Movimento no eixo X/Z
            Vector3 movimento = transform.forward * velocidadeMovimento;
            rb.velocity = new Vector3(movimento.x, rb.velocity.y, movimento.z);
        }
        else
        {
            // Para o movimento se não houver input
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        // Aplica queda mais rápida se estiver no ar
        if (!EstaNoChao())
        {
            rb.velocity += Vector3.down * forcaQueda * Time.fixedDeltaTime;
        }
    }

    bool EstaNoChao()
    {
        // Raycast para verificar se está no chão
        return Physics.Raycast(transform.position, Vector3.down, distanciaVerificacaoChao);
        
    }


    
}
