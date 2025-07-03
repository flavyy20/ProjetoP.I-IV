using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicoptero : MonoBehaviour
{

    [Header("Configurações de Movimento")]
    public float velocidadeHorizontal = 5f;
    public float velocidadeVertical = 3f;
    public float alturaVoo = 10f;
    public float alturaResgate = 3f;
    public float sensibilidadeInclinacao = 20f;

    [Header("Estados")]
    [SerializeField] private EstadoHelicoptero estadoAtual;

    private Transform alvo;
    private Quaternion rotacaoInicial;
    private Vector3 posicaoInicial;

    private enum EstadoHelicoptero
    {
        Parado,
        MovendoParaAlvo,
        DescendoParaResgate,
        SubindoAposResgate
    }

    void Start()
    {
        rotacaoInicial = transform.rotation;
        posicaoInicial = transform.position;
        estadoAtual = EstadoHelicoptero.Parado;

        // Inicia na altura de voo
        transform.position = new Vector3(transform.position.x, alturaVoo, transform.position.z);
    }

    void Update()
    {
        switch (estadoAtual)
        {
            case EstadoHelicoptero.MovendoParaAlvo:
                MoverParaAlvo();
                break;

            case EstadoHelicoptero.DescendoParaResgate:
                DescerParaResgate();
                break;

            case EstadoHelicoptero.SubindoAposResgate:
                SubirAposResgate();
                break;
        }
    }

    void MoverParaAlvo()
    {
        if (alvo == null) return;

        // Calcula posição alvo (mesma altura de voo)
        Vector3 posicaoAlvo = new Vector3(alvo.position.x, alturaVoo, alvo.position.z);

        // Move horizontalmente
        transform.position = Vector3.MoveTowards(transform.position, posicaoAlvo, velocidadeHorizontal * Time.deltaTime);

        // Inclinação durante o movimento
        Vector3 direcao = (posicaoAlvo - transform.position).normalized;
        float inclinacaoFrente = Mathf.Clamp(direcao.z * sensibilidadeInclinacao, -30, 30);
        float inclinacaoLado = Mathf.Clamp(-direcao.x * sensibilidadeInclinacao, -30, 30);
        transform.rotation = Quaternion.Euler(inclinacaoFrente, transform.eulerAngles.y, inclinacaoLado);

        // Verifica se chegou acima do NPC
        if (Vector3.Distance(transform.position, posicaoAlvo) < 0.1f)
        {
            estadoAtual = EstadoHelicoptero.DescendoParaResgate;
        }
    }

    void DescerParaResgate()
    {
        // Desce verticalmente
        float novaAltura = Mathf.MoveTowards(transform.position.y, alturaResgate, velocidadeVertical * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, novaAltura, transform.position.z);

        // Suaviza a rotação durante a descida
        transform.rotation = Quaternion.Lerp(transform.rotation, rotacaoInicial, Time.deltaTime * 2f);

        // Verifica se chegou na altura de resgate
        if (Mathf.Abs(transform.position.y - alturaResgate) < 0.1f)
        {
            ResgatarNPC();
            estadoAtual = EstadoHelicoptero.SubindoAposResgate;
        }
    }

    void SubirAposResgate()
    {
        // Sobe verticalmente
        float novaAltura = Mathf.MoveTowards(transform.position.y, alturaVoo, velocidadeVertical * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, novaAltura, transform.position.z);

        // Verifica se voltou à altura de voo
        if (Mathf.Abs(transform.position.y - alturaVoo) < 0.1f)
        {
            estadoAtual = EstadoHelicoptero.Parado;
            alvo = null;
        }
    }

    void ResgatarNPC()
    {
        Debug.Log($"Iniciando resgate do NPC: {alvo.name}");
        NPC npc = alvo.GetComponent<NPC>();
        if (alvo != null)
        {
            //NPC npc = alvo.GetComponent<NPC>();
            if (npc != null)
            {
                npc.Resgatar();
            }
        }
    }

    public void IniciarResgate(Transform npcAlvo)
    {
        if (estadoAtual == EstadoHelicoptero.Parado)
        {
            alvo = npcAlvo;
            estadoAtual = EstadoHelicoptero.MovendoParaAlvo;
        }
    }


    


}
