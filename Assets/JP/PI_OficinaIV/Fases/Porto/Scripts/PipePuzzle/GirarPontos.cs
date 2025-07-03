using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Right, Down, Left }

public class GirarPontos : MonoBehaviour
{
    [SerializeField] private List<Direction> conexoes = new List<Direction>();
    [SerializeField] private int posicaoX, posicaoY;
    [SerializeField] private bool travado = false;

    public List<Direction> Conexoes => conexoes;

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

        transform.Rotate(0f, 0f, 90f);
        ConectarRotacaoHorario();
    }

    public void ConectarRotacaoHorario()
    {
        for (int i = 0; i < conexoes.Count; i++)
        {
            conexoes[i] = (Direction)(((int)conexoes[i] + 3) % 4);
        }
    }

    public Vector2Int PosicaoGrid()
    {
        return new Vector2Int(posicaoX, posicaoY);
    }
}
