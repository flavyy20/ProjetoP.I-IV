using System.Collections;
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
    public GameObject fundoPrefab;

    public int largura;
    public int altura;
    public float espaco;

    private GirarPontos[,] grid;

    private Vector2Int pontoInicio;
    private Vector2Int pontoFim;

    void Start()
    {
        grid = new GirarPontos[largura, altura];

        pontoInicio = GerarPosicaoNaBorda();
        do
        {
            pontoFim = GerarPosicaoNaBorda();
        } while (pontoFim == pontoInicio);

        CriarFundo();
        GerarCaminhoComSolucao();

        Debug.Log($"Ponto Início: {pontoInicio}, Ponto Fim: {pontoFim}");

        // Opcional: testar se o grid está bem configurado
        ValidarConexoesGrid();
    }

    void CriarFundo()
    {
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                Vector3 pos = new Vector3(x * espaco, y * espaco, 1); // Fundo atrás
                GameObject fundo = Instantiate(fundoPrefab, pos, Quaternion.identity, this.transform);

                var sr = fundo.GetComponent<SpriteRenderer>();
                if (new Vector2Int(x, y) == pontoInicio)
                    sr.color = Color.green;
                else if (new Vector2Int(x, y) == pontoFim)
                    sr.color = Color.red;
            }
        }
    }

    void GerarCaminhoComSolucao()
    {
        List<Vector2Int> caminho = GerarCaminhoEntre(pontoInicio, pontoFim);

        for (int i = 0; i < caminho.Count; i++)
        {
            Vector2Int atual = caminho[i];
            List<Direction> conexoes = new List<Direction>();

            // Conexão com peça anterior
            if (i > 0)
            {
                Vector2Int anterior = caminho[i - 1];
                Direction dirDeAntes = ObterDirecao(atual, anterior);
                conexoes.Add(dirDeAntes);
            }

            // Conexão com próxima peça
            if (i < caminho.Count - 1)
            {
                Vector2Int proximo = caminho[i + 1];
                Direction dirParaProximo = ObterDirecao(atual, proximo);
                conexoes.Add(dirParaProximo);
            }

            // Ajustar ponto final: conexão única para fora
            if (atual == pontoFim)
            {
                conexoes.Clear();
                if (atual.x == 0)
                    conexoes.Add(Direction.Left);
                else if (atual.x == largura - 1)
                    conexoes.Add(Direction.Right);
                else if (atual.y == 0)
                    conexoes.Add(Direction.Down);
                else if (atual.y == altura - 1)
                    conexoes.Add(Direction.Up);
            }

            // Escolher o prefab certo
            GameObject prefab = EhReto(conexoes) ? canoRetoPrefab : canoCurvoPrefab;

            Vector3 pos = new Vector3(atual.x * espaco, atual.y * espaco, 0);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, this.transform);

            GirarPontos gp = obj.GetComponent<GirarPontos>();
            gp.SetarConexoes(conexoes);
            gp.SetarPosicao(atual.x, atual.y);

            // Rotacionar visualmente
            if (atual == pontoFim || atual == pontoInicio)
            {
                Direction dir = conexoes[0]; // única direção

                float angulo = dir switch
                {
                    Direction.Up => 0f,
                    Direction.Right => -90f,
                    Direction.Down => -180f,
                    Direction.Left => -270f,
                    _ => 0f
                };

                obj.transform.rotation = Quaternion.Euler(0f, 0f, angulo);
            }
            else // peças internas: rotacionar aleatoriamente
            {
                int rotacoes = Random.Range(0, 4);
                obj.transform.Rotate(0, 0, -90 * rotacoes);
                for (int r = 0; r < rotacoes; r++)
                    gp.ConectarRotacao();
            }

            grid[atual.x, atual.y] = gp;
        }
    }

    Vector2Int GerarPosicaoNaBorda()
    {
        bool bordaHorizontal = Random.value < 0.5f;

        if (bordaHorizontal)
        {
            int y = (Random.value < 0.5f) ? 0 : altura - 1;
            int x = Random.Range(0, largura);
            return new Vector2Int(x, y);
        }
        else
        {
            int x = (Random.value < 0.5f) ? 0 : largura - 1;
            int y = Random.Range(0, altura);
            return new Vector2Int(x, y);
        }
    }

    List<Vector2Int> GerarCaminhoEntre(Vector2Int inicio, Vector2Int fim)
    {
        List<Vector2Int> caminho = new List<Vector2Int>();
        Vector2Int atual = inicio;
        caminho.Add(atual);

        bool horizontalPrimeiro = Random.value < 0.5f;

        if (horizontalPrimeiro)
        {
            while (atual.x != fim.x)
            {
                atual.x += (fim.x > atual.x) ? 1 : -1;
                caminho.Add(atual);
            }
            while (atual.y != fim.y)
            {
                atual.y += (fim.y > atual.y) ? 1 : -1;
                caminho.Add(atual);
            }
        }
        else
        {
            while (atual.y != fim.y)
            {
                atual.y += (fim.y > atual.y) ? 1 : -1;
                caminho.Add(atual);
            }
            while (atual.x != fim.x)
            {
                atual.x += (fim.x > atual.x) ? 1 : -1;
                caminho.Add(atual);
            }
        }

        return caminho;
    }

    Direction ObterDirecao(Vector2Int de, Vector2Int para)
    {
        Vector2Int diff = para - de;
        if (diff == Vector2Int.up) return Direction.Up;
        if (diff == Vector2Int.down) return Direction.Down;
        if (diff == Vector2Int.left) return Direction.Left;
        if (diff == Vector2Int.right) return Direction.Right;
        return Direction.Up; // fallback seguro
    }

    bool EhReto(List<Direction> cons)
    {
        return (cons.Contains(Direction.Left) && cons.Contains(Direction.Right)) ||
               (cons.Contains(Direction.Up) && cons.Contains(Direction.Down));
    }

    // Método para validar se todas as peças conectam corretamente
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

                    // Verificar se está dentro do grid
                    if (vizinhoPos.x < 0 || vizinhoPos.x >= largura || vizinhoPos.y < 0 || vizinhoPos.y >= altura)
                    {
                        Debug.LogWarning($"Peça em ({x},{y}) tem conexão para fora do grid em direção {dir}");
                        continue;
                    }

                    GirarPontos vizinho = grid[vizinhoPos.x, vizinhoPos.y];
                    if (vizinho == null)
                    {
                        Debug.LogWarning($"Peça em ({x},{y}) conecta para ({vizinhoPos.x},{vizinhoPos.y}) que está vazio");
                        continue;
                    }

                    Direction dirOposta = DirecaoUtil.Oposta(dir);
                    if (!vizinho.Conexoes.Contains(dirOposta))
                    {
                        Debug.LogWarning($"Peça em ({x},{y}) conecta para ({vizinhoPos.x},{vizinhoPos.y}) na direção {dir}, mas peça vizinha não tem conexão oposta {dirOposta}");
                    }
                }
            }
        }
        Debug.Log("Validação de conexões finalizada.");
    }
}

/*quero que o ponto de inicio coincida com o caminho a ser percorrido. por exemplo, se, após o ponto inicial, o próximo cano estiver para a direita ou esquerda, o cano inicial deve ser reto. porém, se o próximo cano estiver em cima ou embaixo, o cano deve ser curvado*/