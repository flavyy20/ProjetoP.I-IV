using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Right, Down, Left }

public class GirarPontos : MonoBehaviour
{
    [SerializeField] private List<Direction> conexoes = new List<Direction>();
    [SerializeField] private int posicaoX, posicaoY;

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

    private void OnMouseDown()
    {
        Debug.Log($"Cano clicado na posi��o ({posicaoX}, {posicaoY})");
        transform.Rotate(0f, 0f, -90f); // gira visualmente no sentido hor�rio
        ConectarRotacao();             // gira l�gica das conex�es no sentido hor�rio
    }

    public void ConectarRotacao()
    {
        for (int i = 0; i < conexoes.Count; i++)
        {
            // Rotaciona 90 graus no sentido hor�rio
            conexoes[i] = (Direction)(((int)conexoes[i] + 1) % 4);
        }
    }

    public Vector2Int PosicaoGrid()
    {
        return new Vector2Int(posicaoX, posicaoY);
    }
}

