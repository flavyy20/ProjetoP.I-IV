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

        ValidarConexoesGrid();
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

            bool isInicio = atual == pontoInicio;
            bool isFim = atual == pontoFim;

            if (isInicio)
            {
                Vector2Int proximo = caminho[i + 1];
                Direction dirParaProximo = ObterDirecao(atual, proximo);

                // Primeiro a entrada, depois a borda
                conexoes.Add(dirParaProximo);

                if (atual.x == 0)
                    conexoes.Add(Direction.Left);
                else if (atual.x == largura - 1)
                    conexoes.Add(Direction.Right);
                else if (atual.y == 0)
                    conexoes.Add(Direction.Down);
                else if (atual.y == altura - 1)
                    conexoes.Add(Direction.Up);
            }
            else if (isFim)
            {
                Vector2Int anterior = caminho[i - 1];
                Direction dirDeAntes = ObterDirecao(atual, anterior);

                // Primeiro a entrada, depois a borda
                conexoes.Add(dirDeAntes);

                if (atual.x == 0)
                    conexoes.Add(Direction.Left);
                else if (atual.x == largura - 1)
                    conexoes.Add(Direction.Right);
                else if (atual.y == 0)
                    conexoes.Add(Direction.Down);
                else if (atual.y == altura - 1)
                    conexoes.Add(Direction.Up);
            }
            else
            {
                Vector2Int anterior = caminho[i - 1];
                Vector2Int proximo = caminho[i + 1];

                conexoes.Add(ObterDirecao(atual, anterior));
                conexoes.Add(ObterDirecao(atual, proximo));
            }

            GameObject prefab = (conexoes.Count == 1 || EhReto(conexoes)) ? canoRetoPrefab : canoCurvoPrefab;

            Vector3 pos = new Vector3(atual.x * espaco, atual.y * espaco, 0);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity, this.transform);

            GirarPontos gp = obj.GetComponent<GirarPontos>();
            gp.SetarConexoes(conexoes);
            gp.SetarPosicao(atual.x, atual.y);

            if (isInicio || isFim)
                gp.TravarPeca();

            float angulo = CalcularRotacaoAPartirDasConexoes(conexoes);
            obj.transform.rotation = Quaternion.Euler(0f, 0f, angulo);

            // Aplica rotação aleatória nas peças internas
            if (!isInicio && !isFim)
            {
                int rotacoes = Random.Range(0, 4);
                obj.transform.Rotate(0f, 0f, -90f * rotacoes);
                for (int r = 0; r < rotacoes; r++)
                    gp.ConectarRotacao();
            }

            grid[atual.x, atual.y] = gp;
        }
    }

    float CalcularRotacaoAPartirDasConexoes(List<Direction> conexoes)
    {
        if (conexoes.Count == 1)
        {
            return conexoes[0] switch
            {
                Direction.Up => 0f,
                Direction.Right => -90f,
                Direction.Down => -180f,
                Direction.Left => -270f,
                _ => 0f
            };
        }

        if (conexoes.Count == 2)
        {
            Direction a = conexoes[0];
            Direction b = conexoes[1];
            var par = new HashSet<Direction> { a, b };

            if (par.SetEquals(new[] { Direction.Left, Direction.Right }))
                return -90f;

            if (par.SetEquals(new[] { Direction.Up, Direction.Down }))
                return 0f;

            if (par.SetEquals(new[] { Direction.Up, Direction.Right }))
                return 0f;

            if (par.SetEquals(new[] { Direction.Right, Direction.Down }))
                return -90f;

            if (par.SetEquals(new[] { Direction.Down, Direction.Left }))
                return -180f;

            if (par.SetEquals(new[] { Direction.Left, Direction.Up }))
                return -270f;
        }

        return 0f;
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

        return caminho;
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

                    if (vizinhoPos.x < 0 || vizinhoPos.x >= largura || vizinhoPos.y < 0 || vizinhoPos.y >= altura)
                        continue;

                    GirarPontos vizinho = grid[vizinhoPos.x, vizinhoPos.y];
                    if (vizinho == null) continue;

                    Direction dirOposta = DirecaoUtil.Oposta(dir);
                    if (!vizinho.Conexoes.Contains(dirOposta))
                    {
                        Debug.LogWarning($"Peça em ({x},{y}) conecta para ({vizinhoPos.x},{vizinhoPos.y}) na direção {dir}, mas a vizinha não tem conexão oposta {dirOposta}");
                    }
                }
            }
        }
    }
}
