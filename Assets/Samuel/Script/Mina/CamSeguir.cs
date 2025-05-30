using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSeguir : MonoBehaviour
{

    //public float sensibilidadeMouse = 100f;
    //public Transform jogador;
    //public Vector3 offset = new Vector3(0f, 1.5f, -3f); // Distância da câmera ao jogador
    //public float suavizacao = 5f;

    //private float rotacaoX = 0f;
    //private bool mouseLiberado = false;
    //private Vector3 rotacaoAtual;

    //void Start()
    //{
    //    LiberarMouse();
    //    // Inicializa a rotação com a atual da câmera
    //    rotacaoAtual = transform.eulerAngles;
    //}

    //void LateUpdate() // Usamos LateUpdate para movimentos suaves
    //{
    //    // Controle do mouse
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        if (mouseLiberado) TravarMouse();
    //        else LiberarMouse();
    //    }

    //    // Rotação apenas com mouse travado
    //    if (!mouseLiberado)
    //    {
    //        RotacionarCamera();
    //    }

    //    // Segue o jogador sem parentesco
    //    SeguirJogador();
    //}

    //void SeguirJogador()
    //{
    //    // Calcula a posição desejada (jogador + offset rotacionado)
    //    Vector3 posicaoDesejada = jogador.position +
    //                             jogador.rotation * offset;

    //    // Movimento suave
    //    transform.position = Vector3.Lerp(
    //        transform.position,
    //        posicaoDesejada,
    //        suavizacao * Time.deltaTime
    //    );

    //    // Rotação alinhada ao jogador + rotação vertical
    //    Quaternion rotacaoDesejada = jogador.rotation *
    //                                Quaternion.Euler(rotacaoX, 0, 0);

    //    transform.rotation = Quaternion.Lerp(
    //        transform.rotation,
    //        rotacaoDesejada,
    //        suavizacao * Time.deltaTime
    //    );
    //}

    //void RotacionarCamera()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
    //    float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

    //    rotacaoX -= mouseY;
    //    rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

    //    // Rotaciona apenas o jogador no eixo Y (horizontal)
    //    jogador.Rotate(Vector3.up * mouseX);
    //}

    //void TravarMouse()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //    mouseLiberado = false;
    //}

    //void LiberarMouse()
    //{
    //    Cursor.lockState = CursorLockMode.None;
    //    Cursor.visible = true;
    //    mouseLiberado = true;
    //}

    public float sensibilidadeMouse = 100f;
    public Transform jogador;
    public Transform alvoPuzzle; // Referência para o puzzle (piezômetro)
    public Vector3 offset = new Vector3(0f, 1.5f, -3f);
    public Vector3 offsetPuzzle = new Vector3(0f, 0f, -2f); // Offset específico para o puzzle
    public float suavizacao = 5f;
    public float tempoTransicao = 1f;

    private float rotacaoX = 0f;
    private bool mouseLiberado = false;
    private Vector3 rotacaoAtual;
    private bool focandoPuzzle = false;
    private float tempoFoco;
    private bool emModoPuzzle;

    void Start()
    {
        LiberarMouse();
        rotacaoAtual = transform.eulerAngles;
    }

    void LateUpdate()
    {
        // Controle do mouse
        if (Input.GetMouseButtonDown(1))
        {
            if (mouseLiberado) TravarMouse();
            else LiberarMouse();
        }

        // Alternar foco com tecla F (ou outra de sua escolha)
        if (Input.GetKeyDown(KeyCode.F))
        {
            focandoPuzzle = !focandoPuzzle;
            tempoFoco = 0f;
        }

        // Atualiza o tempo de transição
        if (tempoFoco < tempoTransicao)
        {
            tempoFoco += Time.deltaTime;
        }

        // Rotaçao apenas com mouse travado e não focando no puzzle
        if (!mouseLiberado && !focandoPuzzle)
        {
            RotacionarCamera();
        }

        // Segue o alvo apropriado
        if (focandoPuzzle)
        {
            FocarPuzzle();
        }
        else
        {
            SeguirJogador();
        }
    }

    void SeguirJogador()
    {
        float percentual = Mathf.Clamp01(tempoFoco / tempoTransicao);

        Vector3 posicaoDesejada = jogador.position + jogador.rotation * offset;
        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            suavizacao * Time.deltaTime * percentual
        );

        Quaternion rotacaoDesejada = jogador.rotation * Quaternion.Euler(rotacaoX, 0, 0);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rotacaoDesejada,
            suavizacao * Time.deltaTime * percentual
        );
    }

    void FocarPuzzle()
    {
        if (alvoPuzzle == null) return;

        float percentual = Mathf.Clamp01(tempoFoco / tempoTransicao);

        Vector3 posicaoDesejada = alvoPuzzle.position + alvoPuzzle.rotation * offsetPuzzle;
        transform.position = Vector3.Lerp(
            transform.position,
            posicaoDesejada,
            suavizacao * Time.deltaTime * percentual
        );

        // Olha para o centro do puzzle
        Vector3 direcao = alvoPuzzle.position - transform.position;
        Quaternion rotacaoDesejada = Quaternion.LookRotation(direcao);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rotacaoDesejada,
            suavizacao * Time.deltaTime * percentual
        );
    }

    void RotacionarCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        jogador.Rotate(Vector3.up * mouseX);
    }

    void TravarMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseLiberado = false;
    }

    void LiberarMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseLiberado = true;
    }

    public void PararControle()
    {
        emModoPuzzle = true;
        LiberarMouse(); // Opcional: permite UI
    }

    public void RetomarControle()
    {
        emModoPuzzle = false;
        TravarMouse(); // Volta ao controle normal
    }
}
