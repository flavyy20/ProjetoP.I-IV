using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPuzzle : MonoBehaviour
{
    [Header("Referências")]
    public RectTransform fundoContainer;  
    public RectTransform pecasContainer;   

    [Header("Prefabs")]
    public GameObject tileFundoPrefab;     
    public GameObject startTilePrefab;
    public GameObject endTilePrefab;
    public GameObject straightTilePrefab;
    public GameObject curveTilePrefab;

    [Header("Tamanho do Grid")]
    public int linhas = 5;
    public int colunas = 5;

    [Header("Posições do Caminho")]
    public Vector2Int posicaoInicio = new Vector2Int(0, 0);
    public Vector2Int posicaoFim = new Vector2Int(4, 4); // Exemplo padrão


    private GameObject[,] pecas;

    void Start()
    {
        CriarGrid();

        bool caminhoValido = TentarEncontrarCaminho();
        Debug.Log(caminhoValido ? "Caminho encontrado!" : "Caminho NÃO encontrado!");
    }


    void CriarGrid()
    {
        pecas = new GameObject[linhas, colunas];

        // Limpa conteúdo antigo
        foreach (Transform child in fundoContainer)
            Destroy(child.gameObject);
        foreach (Transform child in pecasContainer)
            Destroy(child.gameObject);

        // Ajusta layout
        AjustarTamanhoContainer(fundoContainer);
        AjustarTamanhoContainer(pecasContainer);

        // Validar posições
        posicaoInicio.x = Mathf.Clamp(posicaoInicio.x, 0, colunas - 1);
        posicaoInicio.y = Mathf.Clamp(posicaoInicio.y, 0, linhas - 1);
        posicaoFim.x = Mathf.Clamp(posicaoFim.x, 0, colunas - 1);
        posicaoFim.y = Mathf.Clamp(posicaoFim.y, 0, linhas - 1);

        for (int y = 0; y < linhas; y++)
        {
            for (int x = 0; x < colunas; x++)
            {
                // Instancia tile de fundo
                GameObject fundo = Instantiate(tileFundoPrefab, fundoContainer);
                fundo.name = $"Fundo_{x}_{y}";

                GameObject prefabUsado;

                if (x == posicaoInicio.x && y == posicaoInicio.y)
                    prefabUsado = startTilePrefab;
                else if (x == posicaoFim.x && y == posicaoFim.y)
                    prefabUsado = endTilePrefab;
                else
                    prefabUsado = Random.value < 0.5f ? straightTilePrefab : curveTilePrefab;

                GameObject peca = Instantiate(prefabUsado, pecasContainer);
                peca.name = $"Peca_{x}_{y}";

                // Setar a posição na peça (ControlPecas)
                ControlPecas cp = peca.GetComponent<ControlPecas>();
                if (cp != null)
                {
                    cp.Posicao = new Vector2Int(x, y);
                }

                pecas[y, x] = peca;
            }
        }
    }


    void AjustarTamanhoContainer(RectTransform container)
    {
        GridLayoutGroup gridLayout = container.GetComponent<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = colunas;

        Vector2 cellSize = gridLayout.cellSize;
        Vector2 spacing = gridLayout.spacing;
        float width = colunas * cellSize.x + (colunas - 1) * spacing.x;
        float height = linhas * cellSize.y + (linhas - 1) * spacing.y;
        container.sizeDelta = new Vector2(width, height);

        container.anchorMin = new Vector2(0.5f, 0.5f);
        container.anchorMax = new Vector2(0.5f, 0.5f);
        container.pivot = new Vector2(0.5f, 0.5f);
        container.anchoredPosition = Vector2.zero;
    }

    public bool TentarEncontrarCaminho()
    {
        if (pecas == null) return false;

        // Pegando o tile inicial
        ControlPecas tileInicio = pecas[posicaoInicio.y, posicaoInicio.x].GetComponent<ControlPecas>();
        ControlPecas tileFim = pecas[posicaoFim.y, posicaoFim.x].GetComponent<ControlPecas>();

        if (tileInicio == null || tileFim == null)
        {
            Debug.LogError("Peças inicial ou final não encontradas");
            return false;
        }

        // Direção de saída do início — assumindo que existe exatamente 1 conexão na peça start
        if (tileInicio.GetConexoes().Count != 1)
        {
            Debug.LogError("Peça inicial deve ter exatamente 1 direção de saída");
            return false;
        }
        Direction direcaoSaida = tileInicio.GetConexoes()[0];

        // Direção de entrada do fim — assumindo que existe exatamente 1 conexão na peça end
        if (tileFim.GetConexoes().Count != 1)
        {
            Debug.LogError("Peça final deve ter exatamente 1 direção de entrada");
            return false;
        }
        Direction direcaoEntradaFinal = tileFim.GetConexoes()[0];

        // Conjunto para evitar visitar a mesma peça duas vezes (evitar ciclos)
        HashSet<ControlPecas> visitados = new HashSet<ControlPecas>();

        // Inicia a busca a partir da posição vizinha do tile inicial na direção de saída
        Vector2Int proxPos = posicaoInicio + DirecaoUtil.ParaVetor(direcaoSaida);

        return BuscarCaminho(tileInicio, proxPos, DirecaoUtil.Oposta(direcaoSaida), visitados);
    }

    private bool BuscarCaminho(ControlPecas atual, Vector2Int posAtual, Direction vindoDe, HashSet<ControlPecas> visitados)
    {
        // Validar posição dentro do grid
        if (posAtual.x < 0 || posAtual.x >= colunas || posAtual.y < 0 || posAtual.y >= linhas)
            return false;

        ControlPecas proxPeca = pecas[posAtual.y, posAtual.x].GetComponent<ControlPecas>();
        if (proxPeca == null) return false;

        if (visitados.Contains(proxPeca)) return false;

        // Verifica se a peça possui conexão para a direção que estamos vindo
        if (!proxPeca.GetConexoes().Contains(vindoDe)) return false;

        visitados.Add(proxPeca);

        // Se a peça for a final, verificar se a direção bate com a entrada dela
        if (posAtual == posicaoFim)
        {
            // Checar se a conexão da peça final está correta (direção de entrada)
            // A direção de entrada da peça final deve ser exatamente a direção por onde chegamos (vindoDe)
            return true;
        }

        // Explora as outras conexões da peça (exceto a direção que veio)
        foreach (var direcao in proxPeca.GetConexoes())
        {
            if (direcao == vindoDe) continue;

            Vector2Int novaPos = posAtual + DirecaoUtil.ParaVetor(direcao);

            if (BuscarCaminho(proxPeca, novaPos, DirecaoUtil.Oposta(direcao), visitados))
                return true;
        }

        return false;
    }
}
