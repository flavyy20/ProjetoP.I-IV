using System.Collections.Generic;
using UnityEngine;

public static class DirecaoUtil
{
    public static Vector2Int ParaVetor(Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Vector2Int(0, 1),
            Direction.Right => new Vector2Int(1, 0),
            Direction.Down => new Vector2Int(0, -1),
            Direction.Left => new Vector2Int(-1, 0),
            _ => Vector2Int.zero,
        };
    }

    public static Direction Oposta(Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }
}

public class CriarGrid : MonoBehaviour
{
    public GameObject canoRetoPrefab;
    public GameObject canoCurvoPrefab;
    public GameObject canoInicioPrefab;
    public GameObject canoFimPrefab;
    public GameObject fundoPrefab;

    public int largura;
    public int altura;
    public float espaco;

    public Vector2Int pontoInicioDefinido;
    public Vector2Int pontoFimDefinido;
    public int minCanosIntermediarios;

    [Header("Direcoes das pontas")]
    public Direction direcaoSaidaInicio;
    public Direction direcaoEntradaFim;

    private GirarPontos[,] grid;

    void Start()
    {
        Debug.Log($"Direção saída início: {direcaoSaidaInicio}");
        Debug.Log($"Direção entrada fim: {direcaoEntradaFim}");

        grid = new GirarPontos[largura, altura];

        pontoInicioDefinido = ClampDentroDoGrid(pontoInicioDefinido);
        pontoFimDefinido = ClampDentroDoGrid(pontoFimDefinido);

        CriarFundo();

        List<Vector2Int> caminho;
        int tentativas = 0;

        do
        {
            caminho = GerarCaminhoEntre(pontoInicioDefinido, pontoFimDefinido);
            tentativas++;

            if (tentativas > 100)
            {
                Debug.LogError("Falha ao gerar caminho com comprimento mínimo.");
                return;
            }

        } while (caminho.Count < minCanosIntermediarios + 2);

        GerarCaminhoComSolucao(caminho);
        Debug.Log($"Início: {pontoInicioDefinido}, Fim: {pontoFimDefinido}");

        ValidarConexoesGrid();
    }

    Vector2Int ClampDentroDoGrid(Vector2Int pos)
    {
        int x = Mathf.Clamp(pos.x, 0, largura - 1);
        int y = Mathf.Clamp(pos.y, 0, altura - 1);
        return new Vector2Int(x, y);
    }

    void CriarFundo()
    {
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                Vector3 pos = new Vector3(x * espaco, y * espaco, 1);
                GameObject fundo = Instantiate(fundoPrefab, pos, Quaternion.identity, this.transform);

                var sr = fundo.GetComponent<SpriteRenderer>();
                if (new Vector2Int(x, y) == pontoInicioDefinido)
                    sr.color = Color.green;
                else if (new Vector2Int(x, y) == pontoFimDefinido)
                    sr.color = Color.red;
            }
        }
    }

    void GerarCaminhoComSolucao(List<Vector2Int> caminho)
    {
        for (int i = 0; i < caminho.Count; i++)
        {
            Vector2Int atual = caminho[i];

            if (!EstaDentroDoGrid(atual))
            {
                Debug.LogWarning($"Ponto {atual} está fora do grid e será ignorado.");
                continue;
            }

            List<Direction> conexoes = new List<Direction>();

            bool isInicio = i == 0;
            bool isFim = i == caminho.Count - 1;

            if (isInicio && i + 1 < caminho.Count)
            {
                Vector2Int proximo = caminho[i + 1];
                conexoes.Add(ObterDirecao(atual, proximo));
            }
            else if (isFim && i - 1 >= 0)
            {
                Vector2Int anterior = caminho[i - 1];
                conexoes.Add(ObterDirecao(atual, anterior));
            }
            else if (i - 1 >= 0 && i + 1 < caminho.Count)
            {
                Vector2Int anterior = caminho[i - 1];
                Vector2Int proximo = caminho[i + 1];
                conexoes.Add(ObterDirecao(atual, anterior));
                conexoes.Add(ObterDirecao(atual, proximo));
            }

            GameObject prefab;
            if (isInicio)
                prefab = canoInicioPrefab;
            else if (isFim)
                prefab = canoFimPrefab;
            else
                prefab = (EhReto(conexoes) ? canoRetoPrefab : canoCurvoPrefab);

            Vector3 pos = new Vector3(atual.x * espaco, atual.y * espaco, 0);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, this.transform);

            GirarPontos gp = obj.GetComponent<GirarPontos>();
            gp.SetarConexoes(conexoes);
            gp.SetarPosicao(atual.x, atual.y);

            if (isInicio || isFim)
                gp.TravarPeca();

            // Use a direção definida no inspector para calcular rotação, sem alterar nada
            Direction direcaoBase = isInicio ? direcaoSaidaInicio : isFim ? direcaoEntradaFim : Direction.Up;

            float angulo = CalcularRotacaoAPartirDasConexoes(conexoes, direcaoBase);
            obj.transform.rotation = Quaternion.Euler(0, 0, angulo);

            grid[atual.x, atual.y] = gp;
        }
    }

    List<Vector2Int> GerarCaminhoEntre(Vector2Int inicio, Vector2Int fim)
    {
        List<Vector2Int> caminho = new List<Vector2Int>();
        HashSet<Vector2Int> visitados = new HashSet<Vector2Int>();

        caminho.Add(inicio);
        visitados.Add(inicio);

        Vector2Int atual = inicio;

        System.Random rng = new System.Random();
        int tentativas = 0;

        while (atual != fim && tentativas < largura * altura * 10)
        {
            List<Vector2Int> vizinhos = new List<Vector2Int>();

            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                Vector2Int prox = atual + DirecaoUtil.ParaVetor(dir);
                if (EstaDentroDoGrid(prox) && !visitados.Contains(prox))
                {
                    vizinhos.Add(prox);
                }
            }

            if (vizinhos.Count == 0)
            {
                if (caminho.Count > 1)
                {
                    caminho.RemoveAt(caminho.Count - 1);
                    atual = caminho[caminho.Count - 1];
                }
                else
                {
                    break;
                }
            }
            else
            {
                Vector2Int escolhido = vizinhos[rng.Next(vizinhos.Count)];
                caminho.Add(escolhido);
                visitados.Add(escolhido);
                atual = escolhido;
            }

            tentativas++;
        }

        if (!caminho.Contains(fim)) caminho.Add(fim);

        return caminho;
    }


    bool EstaDentroDoGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < largura && pos.y >= 0 && pos.y < altura;
    }

    Direction ObterDirecao(Vector2Int de, Vector2Int para)
    {
        Vector2Int diff = para - de;
        if (diff == Vector2Int.up) return Direction.Up;
        if (diff == Vector2Int.down) return Direction.Down;
        if (diff == Vector2Int.left) return Direction.Left;
        if (diff == Vector2Int.right) return Direction.Right;
        return Direction.Up;
    }

    bool EhReto(List<Direction> cons)
    {
        return (cons.Contains(Direction.Left) && cons.Contains(Direction.Right)) ||
               (cons.Contains(Direction.Up) && cons.Contains(Direction.Down));
    }

    float CalcularRotacaoAPartirDasConexoes(List<Direction> conexoes, Direction direcaoPrefabBase)
    {
        if (conexoes.Count == 1)
        {
            Direction destino = conexoes[0];
            int rotacoes = ((int)destino - (int)direcaoPrefabBase + 4) % 4;
            return -90f * rotacoes;
        }
        else if (conexoes.Count == 2)
        {
            bool horizontal = conexoes.Contains(Direction.Left) && conexoes.Contains(Direction.Right);
            bool vertical = conexoes.Contains(Direction.Up) && conexoes.Contains(Direction.Down);

            if (horizontal) return -90f;
            if (vertical) return 0f;

            if (conexoes.Contains(Direction.Up) && conexoes.Contains(Direction.Right)) return 0f;
            if (conexoes.Contains(Direction.Right) && conexoes.Contains(Direction.Down)) return -90f;
            if (conexoes.Contains(Direction.Down) && conexoes.Contains(Direction.Left)) return -180f;
            if (conexoes.Contains(Direction.Left) && conexoes.Contains(Direction.Up)) return -270f;
        }

        return 0f;
    }

    void ValidarConexoesGrid()
    {
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                GirarPontos atual = grid[x, y];
                if (atual == null) continue;

                Vector2Int posAtual = new Vector2Int(x, y);

                foreach (Direction dir in atual.Conexoes)
                {
                    Vector2Int vizinhoPos = posAtual + DirecaoUtil.ParaVetor(dir);

                    if (!EstaDentroDoGrid(vizinhoPos))
                    {
                        Debug.LogWarning($"({x},{y}) conexão para fora ({dir})");
                        continue;
                    }

                    GirarPontos vizinho = grid[vizinhoPos.x, vizinhoPos.y];
                    if (vizinho == null)
                    {
                        Debug.LogWarning($"({x},{y}) conecta para vazio em ({vizinhoPos})");
                        continue;
                    }

                    Direction dirOposta = DirecaoUtil.Oposta(dir);
                    if (!vizinho.Conexoes.Contains(dirOposta))
                    {
                        Debug.LogWarning($"({x},{y}) conexão {dir} sem oposta em ({vizinhoPos})");
                    }
                }
            }
        }

        Debug.Log("Validação de conexões finalizada.");
    }
}
