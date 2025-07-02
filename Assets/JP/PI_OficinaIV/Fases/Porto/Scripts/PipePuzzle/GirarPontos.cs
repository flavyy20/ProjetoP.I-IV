using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Right, Down, Left }

public class GirarPontos : MonoBehaviour
{
    [SerializeField] private List<Direction> conexoes = new List<Direction>();
    [SerializeField] private int posicaoX, posicaoY;
    [SerializeField] private bool travado = false;

    public List<Direction> Conexoes => conexoes;
    public bool Travado => travado;

    public void SetarConexoes(List<Direction> novasConexoes)
    {
        conexoes = new List<Direction>(novasConexoes);
    }

    public void SetarPosicao(int gridX, int gridY)
    {
        posicaoX = gridX;
        posicaoY = gridY;
    }

    public void TravarPeca()
    {
        travado = true;
    }

    private void OnMouseDown()
    {
        if (travado) return;

        Debug.Log($"Cano clicado na posição ({posicaoX}, {posicaoY})");
        transform.Rotate(0f, 0f, -90f); // gira visualmente no sentido horário
        ConectarRotacao();              // gira logicamente as conexões no sentido horário
    }

    public void ConectarRotacao()
    {
        for (int i = 0; i < conexoes.Count; i++)
        {
            conexoes[i] = (Direction)(((int)conexoes[i] + 1) % 4);
        }
    }

    public Vector2Int PosicaoGrid()
    {
        return new Vector2Int(posicaoX, posicaoY);
    }
}
