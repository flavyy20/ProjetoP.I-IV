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

    [Header("Tamanho do Grid")]
    public int linhas = 5;
    public int colunas = 5;

    [Header("Posições do Caminho")]
    public Vector2Int posicaoInicio = new Vector2Int(0, 0);
    public Direction direcaoInicio = Direction.Right;

    public Vector2Int posicaoFim = new Vector2Int(4, 4);
    public Direction direcaoFim = Direction.Left;

    private void Start()
    {
        CriarGrid();
    }

    void CriarGrid()
    {
        // Limpa apenas os fundos
        foreach (Transform child in fundoContainer)
            Destroy(child.gameObject);

        AjustarTamanhoContainer(fundoContainer);
        AjustarTamanhoContainer(pecasContainer);

        // Clampa posições válidas
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

                Vector2Int posAtual = new Vector2Int(x, y);

                if (posAtual == posicaoInicio)
                {
                    GameObject peca = Instantiate(startTilePrefab, pecasContainer);
                    peca.name = $"Start_{x}_{y}";

                    ControlPecas cp = peca.GetComponent<ControlPecas>();
                    if (cp != null)
                    {
                        cp.Posicao = posAtual;
                        cp.usarDirecaoInicial = true;
                        cp.podeGirar = false;
                        cp.direcaoFixaInicial = direcaoInicio;
                    }
                }
                else if (posAtual == posicaoFim)
                {
                    GameObject peca = Instantiate(endTilePrefab, pecasContainer);
                    peca.name = $"End_{x}_{y}";

                    ControlPecas cp = peca.GetComponent<ControlPecas>();
                    if (cp != null)
                    {
                        cp.Posicao = posAtual;
                        cp.usarDirecaoInicial = true;
                        cp.podeGirar = false;
                        cp.direcaoFixaInicial = direcaoFim;
                    }
                }
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
}
