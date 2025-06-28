using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidadeMovimento = 5f;
    public Transform cameraPrincipal;
    public Animator animator; 
    public float velocidadeRotacao= 8f;

    private float rotacaoX = 0f;
    private bool estaCorrendo = false;
    private Vector3 direcaoMovimento;

    void Start()
    {
       
    }

    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        estaCorrendo = (horizontal != 0f || vertical != 0f);

        direcaoMovimento = new Vector3(horizontal, 0f, vertical).normalized;

        if (direcaoMovimento.magnitude >= 0.1f)
        {
            
            float anguloAlvo = Mathf.Atan2(direcaoMovimento.x, direcaoMovimento.z) * Mathf.Rad2Deg;

            
            Quaternion rotacaoAlvo = Quaternion.Euler(0f, anguloAlvo, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeRotacao * Time.deltaTime);

            
            transform.Translate(Vector3.forward * velocidadeMovimento * Time.deltaTime);
        }



        if (animator != null)
        {
            animator.SetBool("Correndo", estaCorrendo);
        }

    }
}
