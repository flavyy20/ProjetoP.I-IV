using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GerarGrid : MonoBehaviour
{
    [Header("Tamanho do Grid")]
    public int linhas = 5;
    public int colunas = 5;
    public float espacoEntrePecas = 1f;

    [Header("Ponto Inicial")]
    public Vector2Int posicaoInicio = new Vector2Int(0, 0);
    public Direction direcaoInicio = Direction.Right;
    public GameObject prefabInicio;

    [Header("Ponto Final")]
    public Vector2Int posicaoFim = new Vector2Int(4, 4);
    public Direction direcaoFim = Direction.Left;
    public GameObject prefabFim;

    [Header("Peças Intermediárias")]
    public GameObject prefabReto;
    public GameObject prefabCurva;

    [Header("Base do Grid")]
    public GameObject prefabBaseGrid;

    private GameObject[,] gridPecas;
    private GameObject[,] gridBases;
    private List<Vector2Int> caminho;
    private Vector3 offsetCentralizacao;

    void Start()
    {
        if (!ValidarPosicoes())
        {
            Debug.LogError("Posições de início ou fim inválidas.");
            return;
        }

        Camera cam = Camera.main;
        Vector3 centroDoGrid = new Vector3(
            (colunas - 1) * espacoEntrePecas / 2f,
            (linhas - 1) * espacoEntrePecas / 2f,
            0
        );
        offsetCentralizacao = cam.transform.position - centroDoGrid;

        gridPecas = new GameObject[linhas, colunas];
        gridBases = new GameObject[linhas, colunas];

        GerarBaseDoGridUI();
        caminho = GerarCaminhoBFS();
        if (caminho == null || caminho.Count == 0)
        {
            Debug.LogError("Caminho não foi gerado. Abortando criação de peças.");
            return;
        }

        GerarPecasDoCaminho();


    }

    bool ValidarPosicoes()
    {
        return posicaoInicio != posicaoFim &&
               DentroDoGrid(posicaoInicio) &&
               DentroDoGrid(posicaoFim);
    }

    bool DentroDoGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < linhas &&
               pos.y >= 0 && pos.y < colunas;
    }

    void GerarBaseDoGridUI()
    {
        for (int linha = 0; linha < linhas; linha++)
        {
            for (int coluna = 0; coluna < colunas; coluna++)
            {
                GameObject baseUI = Instantiate(prefabBaseGrid, transform);
                RectTransform rt = baseUI.GetComponent<RectTransform>();

                // Posiciona no Canvas (anchoredPosition = UI space)
                rt.anchoredPosition = new Vector2(
                    coluna * espacoEntrePecas,
                    -linha * espacoEntrePecas
                );

                gridBases[linha, coluna] = baseUI;
            }
        }

        // Centralizar o grid (aqui é o container pai no Canvas)
        RectTransform pai = GetComponent<RectTransform>();
        float largura = (colunas - 1) * espacoEntrePecas;
        float altura = (linhas - 1) * espacoEntrePecas;
        pai.anchoredPosition = new Vector2(-largura / 2f, altura / 2f);
    }



    List<Vector2Int> GerarCaminhoBFS()
    {
        Queue<List<Vector2Int>> fila = new Queue<List<Vector2Int>>();
        HashSet<Vector2Int> visitados = new HashSet<Vector2Int>();

        Vector2Int dirInicio = DirecaoParaVetor(direcaoInicio);
        Vector2Int primeiro = posicaoInicio + dirInicio;

        if (!DentroDoGrid(primeiro)) return new List<Vector2Int>();

        List<Vector2Int> caminhoInicial = new List<Vector2Int> { posicaoInicio, primeiro };
        fila.Enqueue(caminhoInicial);
        visitados.Add(posicaoInicio);
        visitados.Add(primeiro);

        while (fila.Count > 0)
        {
            var caminhoAtual = fila.Dequeue();
            Vector2Int atual = caminhoAtual[caminhoAtual.Count - 1];

            if (atual == posicaoFim)
            {
                return caminhoAtual;
            }

            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                Vector2Int prox = atual + DirecaoParaVetor(dir);

                if (!DentroDoGrid(prox) || visitados.Contains(prox)) continue;

                // Se for a posição final, só conecta se a direção bater com a entrada final
                if (prox == posicaoFim)
                {
                    Vector2Int vindoDe = atual;
                    Direction entrada = ObterDirecaoEntrePontos(posicaoFim, vindoDe);
                    if (entrada != DirecaoOposta(direcaoFim)) continue;
                }

                List<Vector2Int> novoCaminho = new List<Vector2Int>(caminhoAtual) { prox };
                fila.Enqueue(novoCaminho);
                visitados.Add(prox);
            }
        }

        Debug.LogError("Caminho impossível.");
        return new List<Vector2Int>();
    }

    // ... manter GerarPecasDoCaminho(), ObterDirecaoEntrePontos(), DirecaoOposta(), DirecaoParaVetor(), EmbaralharDirecoes(), CalcularRotacao(), DirecaoParaAngulo() iguais ...

    // Lembre-se de manter também as outras funções auxiliares abaixo conforme seu código original.

    void GerarPecasDoCaminho()
    {
        for (int i = 0; i < caminho.Count; i++)
        {
            Vector2Int pos = caminho[i];

            // Instancia no Canvas UI
            GameObject prefab = null;
            Direction entrada = Direction.Up;
            Direction saida = Direction.Up;
            bool ehCurva = false;

            if (i == 0) // Início
            {
                prefab = prefabInicio;
                entrada = DirecaoOposta(direcaoInicio);
                saida = direcaoInicio;
            }
            else if (i == caminho.Count - 1) // Fim
            {
                prefab = prefabFim;
                entrada = ObterDirecaoEntrePontos(caminho[i], caminho[i - 1]);
                saida = direcaoFim;
            }
            else // Peças intermediárias
            {
                entrada = ObterDirecaoEntrePontos(pos, caminho[i - 1]);
                saida = ObterDirecaoEntrePontos(pos, caminho[i + 1]);

                ehCurva = !SaoOpostas(entrada, saida);
                prefab = ehCurva ? prefabCurva : prefabReto;
            }

            GameObject peca = Instantiate(prefab, transform);
            RectTransform rt = peca.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(
                pos.y * espacoEntrePecas,
                -pos.x * espacoEntrePecas
            );

            // Rotacionar
            int rotacao = (i == 0) ? DirecaoParaAngulo(saida)
                       : (i == caminho.Count - 1) ? DirecaoParaAngulo(entrada)
                       : CalcularRotacao(entrada, saida, ehCurva);
            rt.localRotation = Quaternion.Euler(0, 0, rotacao);

            // Configurar conexões
            GirarPontos gp = peca.GetComponent<GirarPontos>();
            if (gp != null)
            {
                gp.DefinirConexoes(entrada, saida);
                gp.SetarInterativo(i != 0 && i != caminho.Count - 1); // só gira se não for início/fim
            }

            // Colorir início/fim
            Image img = peca.GetComponent<Image>();
            if (img != null)
            {
                if (i == 0) img.color = Color.green;
                else if (i == caminho.Count - 1) img.color = Color.red;
            }
        }
    }


    Direction ObterDirecaoEntrePontos(Vector2Int de, Vector2Int para)
    {
        Vector2Int delta = para - de;

        if (delta == Vector2Int.up) return Direction.Up;
        if (delta == Vector2Int.right) return Direction.Right;
        if (delta == Vector2Int.down) return Direction.Down;
        if (delta == Vector2Int.left) return Direction.Left;

        Debug.LogWarning("Pontos não adjacentes!");
        return Direction.Up;
    }

    Direction DirecaoOposta(Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }

    Vector2Int DirecaoParaVetor(Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Vector2Int(-1, 0),
            Direction.Right => new Vector2Int(0, 1),
            Direction.Down => new Vector2Int(1, 0),
            Direction.Left => new Vector2Int(0, -1),
            _ => Vector2Int.zero
        };
    }

    List<Direction> EmbaralharDirecoes(List<Direction> direcoes, System.Random rng)
    {
        for (int i = 0; i < direcoes.Count; i++)
        {
            int k = rng.Next(i, direcoes.Count);
            (direcoes[i], direcoes[k]) = (direcoes[k], direcoes[i]);
        }
        return direcoes;
    }

    int CalcularRotacao(Direction entrada, Direction saida, bool ehCurva)
    {
        if (!ehCurva)
        {
            if ((entrada == Direction.Up && saida == Direction.Down) || (entrada == Direction.Down && saida == Direction.Up))
                return 0;
            if ((entrada == Direction.Left && saida == Direction.Right) || (entrada == Direction.Right && saida == Direction.Left))
                return 90;
        }
        else
        {
            if ((entrada == Direction.Up && saida == Direction.Right) || (entrada == Direction.Right && saida == Direction.Up))
                return 0;
            if ((entrada == Direction.Right && saida == Direction.Down) || (entrada == Direction.Down && saida == Direction.Right))
                return 90;
            if ((entrada == Direction.Down && saida == Direction.Left) || (entrada == Direction.Left && saida == Direction.Down))
                return 180;
            if ((entrada == Direction.Left && saida == Direction.Up) || (entrada == Direction.Up && saida == Direction.Left))
                return 270;
        }

        Debug.LogWarning($"Rotação indefinida para: Entrada {entrada}, Saída {saida}");
        return 0;
    }

    int DirecaoParaAngulo(Direction dir)
    {
        return dir switch
        {
            Direction.Up => 0,
            Direction.Right => -90,
            Direction.Down => 180,
            Direction.Left => 90,
            _ => 0
        };
    }

    bool SaoOpostas(Direction a, Direction b)
    {
        return DirecaoOposta(a) == b;
    }

}
