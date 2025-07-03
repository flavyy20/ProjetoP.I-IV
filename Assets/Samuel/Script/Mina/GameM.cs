using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameM : MonoBehaviour
{
    public static GameM instance;

    [Header("Gerenciamento de Fases")]
    public Fase faseAtual = Fase.PreDesastre;
    public enum Fase { PreDesastre, PosDesastre }

    [Header("Temporizador Pré-Desastre")]
    public float tempoMaximoPreDesastre = 300f;
    public float tempoDecorrido = 0f;
    public Text textoTempo;

    [Header("Pontos de Fuga")]
    public List<PontoDeFuga> pontosFugaSelecionados = new List<PontoDeFuga>();
    public int totalNPCsNosPontos = 0;
    public static int npcsResgatados = 0;

    [Header("UI")]
    public GameObject uiPontosFuga;
    public Text textoNPCsResgatados;

    [Header("Configuração de Cenas")]
    public string cenaPosDesastre = "MinaPosDesastre";

    private bool transicaoEmAndamento = false;
    public NPC npcres; 

    [System.Serializable]
    public class DadosPersistentes
    {
        public List<PontoFugaData> pontosData = new List<PontoFugaData>();
        public List<DadosNPC> npcsData = new List<DadosNPC>();
        public static DadosPersistentes Instancia;
    }

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DadosPersistentes.Instancia = new DadosPersistentes();
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Resetar contadores ao iniciar
            npcsResgatados = 0;
            totalNPCsNosPontos = 0;
        }
        
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (faseAtual == Fase.PreDesastre && !transicaoEmAndamento)
        {
            AtualizarTemporizador();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            tempoDecorrido = tempoMaximoPreDesastre - 10;
        }
       

        Debug.Log($"Total NPCs nos pontos: {totalNPCsNosPontos}");
        Debug.Log($"NPCs resgatados: {npcsResgatados}");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == cenaPosDesastre)
        {
            faseAtual = Fase.PosDesastre;
            transicaoEmAndamento = false;
            StartCoroutine(CarregarNPCsComDelay());
        }
    }

    IEnumerator CarregarNPCsComDelay()
    {
        
        yield return null; // Espera um frame

        // Limpa NPCs existentes (limpeza opcional)
        NPC[] npcsExistentes = FindObjectsOfType<NPC>();
        foreach (NPC npc in npcsExistentes)
        {
            Destroy(npc.gameObject);
        }

        // Reconstroi os pontos de fuga selecionados
        pontosFugaSelecionados.Clear();
        foreach (var data in DadosPersistentes.Instancia.pontosData)
        {
            PontoDeFuga ponto = GameObject.Find(data.nomePonto)?.GetComponent<PontoDeFuga>();
            if (ponto != null)
            {
                ponto.npcsPresentes = data.npcsPresentes;
                ponto.selecionado = data.selecionado;
                pontosFugaSelecionados.Add(ponto);

                if (ponto.selecionado)
                {
                    // Spawna NPCs manualmente (sem depender de dados complexos)
                    ponto.CarregarNPCsManual();
                    Debug.Log($"NPCs spawnados no ponto: {ponto.name}");
                }
            }
        }

        AtualizarUI();
    }


    void AtualizarTemporizador()
    {
        tempoDecorrido += Time.deltaTime;

        if (textoTempo != null)
        {
            float tempoRestante = Mathf.Max(0, tempoMaximoPreDesastre - tempoDecorrido);
            textoTempo.text = $"Tempo restante: {FormatarTempo(tempoRestante)}";
        }

        if (tempoDecorrido >= tempoMaximoPreDesastre && !transicaoEmAndamento)
        {
            IniciarPosDesastre();
        }
    }

    string FormatarTempo(float segundos)
    {
        int minutos = Mathf.FloorToInt(segundos / 60);
        int segs = Mathf.FloorToInt(segundos % 60);
        return string.Format("{0:00}:{1:00}", minutos, segs);
    }

    public void IniciarPosDesastre()
    {
        



        if (transicaoEmAndamento) return;
        transicaoEmAndamento = true;

        // Prepara os dados para a próxima cena
        PrepararTransicao();
        DadosPersistentes.Instancia.pontosData = pontosFugaData;
        DadosPersistentes.Instancia.npcsData = npcsParaSpawnar;

        SceneManager.LoadScene(cenaPosDesastre);

    }

    // Chamado quando seleciona um ponto no pré-desastre
    public void AdicionarPontoFuga(PontoDeFuga ponto)
    {
        if (!pontosFugaSelecionados.Contains(ponto))
        {
            pontosFugaSelecionados.Add(ponto);
            totalNPCsNosPontos += ponto.npcsPresentes;
            AtualizarUI();
            Debug.Log($"Ponto adicionado! Total: {pontosFugaSelecionados.Count}");
        }
    }

    // Chamado quando desseleciona um ponto no pré-desastre
    public void RemoverPontoFuga(PontoDeFuga ponto)
    {
        if (pontosFugaSelecionados.Contains(ponto))
        {
            pontosFugaSelecionados.Remove(ponto);
            totalNPCsNosPontos -= ponto.npcsPresentes;
            AtualizarUI();
            Debug.Log($"Ponto removido! Restantes: {pontosFugaSelecionados.Count}");
        }
    }

    // Chamado ao resgatar NPCs no pós-desastre
    public void AtualizarNPCsResgatados(int quantidade)
    {

        npcsResgatados += quantidade;
        AtualizarUI();
        Debug.Log($"NPCs resgatados: {npcsResgatados}/{totalNPCsNosPontos}");

        // Verificação corrigida
        if (npcsResgatados >= totalNPCsNosPontos && totalNPCsNosPontos > 0)
        {
            Debug.Log("Todos NPCs resgatados! Missão cumprida!");
            StartCoroutine(CarregarTelaMonitoramentoComDelay());
        }
    }
    FaseController faseCon;
    IEnumerator CarregarTelaMonitoramentoComDelay()
    {
        //faseCon.PerderCidade();
        yield return new WaitForSeconds(2f); // Espera 2 segundos para mostrar efeitos visuais
        SceneManager.LoadScene("TelaMonitoramento");
    }

   


    // Atualiza elementos da UI
    void AtualizarUI()
    {
        if (uiPontosFuga != null)
        {
            uiPontosFuga.GetComponent<Text>().text =
                $"Pontos selecionados: {pontosFugaSelecionados.Count}";
        }

        if (textoNPCsResgatados != null && faseAtual == Fase.PosDesastre)
        {
            textoNPCsResgatados.text =
                $"Resgatados: {npcsResgatados}/{totalNPCsNosPontos}";
        }
    }

   


    [System.Serializable]
    public class DadosNPC
    {
        public string pontoFugaID;
        public Vector3 posicao;
        public bool resgatado;
    }

    [System.Serializable]
    public class PontoFugaData
    {
        public string nomePonto;
        public int npcsPresentes;
        public Vector3 posicao;
        public bool selecionado;
    }

    public List<DadosNPC> npcsParaSpawnar = new List<DadosNPC>();
    public List<PontoFugaData> pontosFugaData = new List<PontoFugaData>();
    public void PrepararTransicao()
    {
        npcsParaSpawnar.Clear();
        pontosFugaData.Clear();

        totalNPCsNosPontos = 0; // Reset antes de recalcular

        foreach (PontoDeFuga ponto in pontosFugaSelecionados)
        {
            totalNPCsNosPontos += ponto.npcsPresentes;
            foreach (NPC npc in ponto.npcsNoPonto)
            {
                npcsParaSpawnar.Add(new DadosNPC()
                {
                    pontoFugaID = ponto.name,
                    posicao = npc.transform.position,
                    resgatado = npc.resgatado
                });
            }
        }

        pontosFugaData.Clear();

        foreach (PontoDeFuga ponto in pontosFugaSelecionados)
        {
            pontosFugaData.Add(new PontoFugaData()
            {
                nomePonto = ponto.name,
                npcsPresentes = ponto.npcsPresentes,
                posicao = ponto.transform.position,
                selecionado = ponto.selecionado
            });
        }
    }

    void ReconstruirReferenciasPosDesastre()
    {
        pontosFugaSelecionados.Clear();

        foreach (PontoFugaData data in pontosFugaData)
        {
            PontoDeFuga ponto = GameObject.Find(data.nomePonto)?.GetComponent<PontoDeFuga>();

            if (ponto != null)
            {
                ponto.npcsPresentes = data.npcsPresentes;
                ponto.transform.position = data.posicao;
                ponto.selecionado = data.selecionado;

                pontosFugaSelecionados.Add(ponto);

                // CHAMADA CORRETA DO MÉTODO ATUALIZADO:
                if (ponto.selecionado)
                {
                    List<DadosNPC> npcsDestePonto = npcsParaSpawnar
                        .Where(n => n.pontoFugaID == data.nomePonto)
                        .ToList();

                    ponto.SpawnarNPCsPosDesastre(npcsDestePonto);
                }
            }
        }
    }

    


    public void PrepararParaMudancaCena()
    {
        npcsParaSpawnar.Clear();

        foreach (PontoDeFuga ponto in pontosFugaSelecionados)
        {
            foreach (NPC npc in ponto.npcsNoPonto)
            {
                npcsParaSpawnar.Add(new GameM.DadosNPC()
                {
                    pontoFugaID = ponto.name,
                    posicao = npc.transform.position,
                    resgatado = npc.resgatado
                });
            }
        }
    }

   
    
}
