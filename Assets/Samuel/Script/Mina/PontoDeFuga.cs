using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameM;

public class PontoDeFuga : MonoBehaviour
{
    [Header("Configurações Pré-Desastre")]
    public Material materialSelecionado;      // Material quando selecionado como ponto de fuga
    public float distanciaInteracao = 3f;     // Distância para interagir com o ponto
    public bool selecionado = false;          // Foi marcado como ponto de fuga?

    [Header("Configurações Pós-Desastre")]
    public int npcsPresentes = 3;             // Quantidade de NPCs que aparecerão aqui
    public GameObject prefabNPC;              // Prefab do NPC
    public float raioResgate = 5f;            // Distância para resgatar NPCs
    public Material materialResgatado;        // Material quando todos NPCs forem resgatados

    [Header("Efeitos Visuais")]
    public GameObject efeitoResgate;          // Partículas ao resgatar NPCs

    private Material materialOriginal;
    [SerializeField]
    public  List<NPC> npcsNoPonto = new List<NPC>();
    [SerializeField]
    private bool todosResgatados = false;

    void Start()
    {
        

        if (GameM.instance.faseAtual == GameM.Fase.PosDesastre && selecionado)
        {
            CarregarNPCsPosDesastre();
        }
    }

    void CarregarNPCsPosDesastre()
    {
        if (DadosPersistentes.Instancia == null) return;

        var npcsDestePonto = DadosPersistentes.Instancia.npcsData
            .Where(n => n.pontoFugaID == this.name)
            .ToList();

        foreach (var dados in npcsDestePonto)
        {
            GameObject npc = Instantiate(
                prefabNPC,
                dados.posicao,
                Quaternion.identity,
                transform);

            NPC npcScript = npc.GetComponent<NPC>();
            npcScript.resgatado = dados.resgatado;
            npcsNoPonto.Add(npcScript);

            if (dados.resgatado)
            {
                npc.SetActive(false);
            }
        }
    }

    public void CarregarNPCsManual()
    {
        // Limpa NPCs existentes
        foreach (NPC npc in npcsNoPonto.ToArray())
        {
            if (npc != null) Destroy(npc.gameObject);
        }
        npcsNoPonto.Clear();

        // Spawna os NPCs diretamente
        for (int i = 0; i < npcsPresentes; i++)
        {
            Vector3 posicao = transform.position + new Vector3(
                Random.Range(-2f, 2f),
                0,
                Random.Range(-2f, 2f));

            GameObject npc = Instantiate(
                prefabNPC,
                posicao,
                Quaternion.identity,
                transform);

            npc.transform.localScale = prefabNPC.transform.localScale;
            npc.name = $"NPC_{name}_{i}";
            npcsNoPonto.Add(npc.GetComponent<NPC>());
        }
    }

    void Update()
    {
        if (GameM.instance == null) return;

        // COMPORTAMENTO PRÉ-DESASTRE (seleção de pontos)
        if (GameM.instance.faseAtual == GameM.Fase.PreDesastre)
        {
            float distancia = Vector3.Distance(transform.position,
                GameObject.FindGameObjectWithTag("Player").transform.position);

            if (distancia <= distanciaInteracao && Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Ta apertando R");
                AlternarSelecao();
            }
        }
        // COMPORTAMENTO PÓS-DESASTRE (resgate de NPCs)
        else if (GameM.instance.faseAtual == GameM.Fase.PosDesastre &&
                selecionado &&
                !todosResgatados &&
                Input.GetKeyDown(KeyCode.T))
        {
            TentarResgatarNPCs();
        }
    }

    // PRÉ-DESASTRE: Seleciona/desseleciona o ponto
    void AlternarSelecao()
    {
        selecionado = !selecionado;
        GetComponent<Renderer>().material = selecionado ? materialSelecionado : materialOriginal;

        if (GameM.instance != null)
        {
            if (selecionado)
                GameM.instance.AdicionarPontoFuga(this);
            else
                GameM.instance.RemoverPontoFuga(this);
        }

        Debug.Log("Ponto " + (selecionado ? "selecionado" : "desselecionado"));
    }


    public void SpawnarNPCsPosDesastre(List<GameM.DadosNPC> npcsDados)
    {
        /// Limpa NPCs existentes
        foreach (var npc in npcsNoPonto.ToArray())
        {
            if (npc != null) Destroy(npc.gameObject);
        }
        npcsNoPonto.Clear();

        // Spawn dos NPCs salvos
        foreach (var dados in npcsDados)
        {
            GameObject npc = Instantiate(
                prefabNPC,
                dados.posicao,
                Quaternion.identity,
                transform);

            NPC npcScript = npc.GetComponent<NPC>();
            npcScript.resgatado = dados.resgatado;
            npcsNoPonto.Add(npcScript);

            if (dados.resgatado)
            {
                npc.SetActive(false);
                // Ou Destroy(npc) se não for mais necessário
            }
        }
    }

    // PÓS-DESASTRE: Tenta resgatar NPCs próximos
    void TentarResgatarNPCs()
    {

        float distanciaJogador = Vector3.Distance(transform.position,
       GameObject.FindGameObjectWithTag("Player").transform.position);

        if (distanciaJogador > raioResgate) return;

        int npcsResgatadosNestaChamada = 0;
        foreach (NPC npc in npcsNoPonto.ToArray()) // Usar ToArray para evitar modificação durante iteração
        {
            if (npc != null && !npc.resgatado)
            {
                npc.Resgatar();
                npcsResgatadosNestaChamada++;
            }
        }

        if (npcsResgatadosNestaChamada > 0)
        {
            if (efeitoResgate != null)
                Instantiate(efeitoResgate, transform.position, Quaternion.identity);

            todosResgatados = npcsNoPonto.All(n => n == null || n.resgatado);

            if (todosResgatados)
            {
                GetComponent<Renderer>().material = materialResgatado;
                GameM.instance.AtualizarNPCsResgatados(npcsResgatadosNestaChamada);
            }
        }

       
    }

    void OnDrawGizmosSelected()
    {
        // Pré-desastre: raio de interação (verde)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, distanciaInteracao);

        // Pós-desastre: raio de resgate (azul)
        if (GameM.instance != null && GameM.instance.faseAtual == GameM.Fase.PosDesastre)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, raioResgate);
        }
    }
}
