using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum Direction { Up, Right, Down, Left }

public class GirarPontos : MonoBehaviour
{
    [SerializeField] private List<Direction> conexoes = new List<Direction>();
    [SerializeField] private bool interativo = true;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnClick()
    {
        if (!interativo) return;

        rectTransform.Rotate(0, 0, -90f); // Roda 90° sentido horário

        for (int i = 0; i < conexoes.Count; i++)
        {
            conexoes[i] = (Direction)(((int)conexoes[i] + 1) % 4); // Roda lógica também
        }
    }

    public void DefinirConexoes(Direction entrada, Direction saida)
    {
        conexoes.Clear();
        conexoes.Add(entrada);
        conexoes.Add(saida);
    }

    public void SetarInterativo(bool ativo)
    {
        interativo = ativo;
    }

    public List<Direction> ObterConexoes()
    {
        return conexoes;
    }
}
