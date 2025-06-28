using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJogador : MonoBehaviour
{
    InputManager inputManager;

    Vector3 direcao;
    Transform cameraPersonagem;
    Rigidbody rg;

    [SerializeField] float velocidade, velRotacao;

    [Header("Fisica")]
    [SerializeField] bool detectarChao;
    [SerializeField] float velQueda, forcaPulo;
    [SerializeField] LayerMask layerColisao;

    float tempoAr;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rg = GetComponent<Rigidbody>();
        cameraPersonagem = Camera.main.transform;
    }

    private void Movimentacao()
    {
        direcao = cameraPersonagem.forward * inputManager.inputVertical;
        direcao += cameraPersonagem.right * inputManager.inputHorizontal;
        direcao.Normalize();

        direcao.y = 0;
        direcao *= velocidade;

        Vector3 velocidadeDirecao = direcao;
        rg.velocity = velocidadeDirecao;
    }

    private void Rotacao()
    {
        Vector3 direcaoRotacao = Vector3.zero;

        direcaoRotacao = cameraPersonagem.forward * inputManager.inputVertical;
        direcaoRotacao += cameraPersonagem.right * inputManager.inputHorizontal;
        direcao.Normalize();

        direcaoRotacao.y = 0;

        if (direcaoRotacao == Vector3.zero)
            direcaoRotacao = transform.forward;

        Quaternion rotacaoPlayer = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direcaoRotacao), velRotacao * Time.deltaTime);
        transform.rotation = rotacaoPlayer;
    }

    public void RodarInputsJogador()
    {
        Movimentacao();
        Rotacao();
        FisicaPersonagem();
    }

    void FisicaPersonagem()
    {
        RaycastHit hit;
        Vector3 pontoOrigem = transform.position;
        pontoOrigem.y += 0.5f;

        if(!detectarChao)
        {
            tempoAr *= Time.deltaTime;
            rg.AddForce(transform.forward * forcaPulo);
            rg.AddForce(-Vector3.up * velQueda * tempoAr);
        }

        if(Physics.SphereCast(pontoOrigem, 0.2f, -Vector3.up, out hit, layerColisao))
        {
            tempoAr = 0;
            detectarChao = true;
        }
        else
            detectarChao = false;
    }
}

//CharacterController cControler;

//[SerializeField] float velocidade;
//[SerializeField] float forcaPulo;

//[SerializeField] Transform pePersonagem;
//[SerializeField] LayerMask colisaoPulo;
//bool detectarChao;

//void Start()
//{
//    cControler = GetComponent<CharacterController>();
//}

//// Update is called once per frame
//void Update()
//{
//    Movimentacao();

//    if (Input.GetKeyDown(KeyCode.Space) && detectarChao)
//    {
//        Pular();
//    }
//}

//void Movimentacao()
//{
//    Vector3 direcaoMovimento = Vector3.zero;
//    foreach (KeyCode key in new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D })
//    {
//        if (Input.GetKey(key))
//        {
//            switch (key)
//            {
//                case KeyCode.W:
//                    direcaoMovimento += Vector3.forward;
//                    break;
//                case KeyCode.A:
//                    direcaoMovimento += Vector3.left;
//                    break;
//                case KeyCode.S:
//                    direcaoMovimento += Vector3.back;
//                    break;
//                case KeyCode.D:
//                    direcaoMovimento += Vector3.right;
//                    break;
//            }
//        }
//    }

//    direcaoMovimento = direcaoMovimento.normalized;
//    cControler.Move(direcaoMovimento * velocidade * Time.deltaTime);
//}

//void Pular()
//{
//    detectarChao = Physics.CheckSphere(pePersonagem.position, 0.1f, colisaoPulo);
//    cControler.Move(new Vector3(0, forcaPulo, 0) * Time.deltaTime);

//    if (forcaPulo > -9.81f)
//    {
//        forcaPulo += -9.81f * Time.deltaTime;
//    }

//    //if()
//    //if(cControler.isGrounded)
//    //{

//    //}
//}
