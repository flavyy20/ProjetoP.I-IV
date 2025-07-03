using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ControlPecas : MonoBehaviour, IPointerClickHandler
{
    [Header("Configuração")]
    public bool podeGirar = true;
    [SerializeField] private List<Direction> conexoesOriginais = new List<Direction>();

    [Header("Direção Inicial (para Start/End)")]
    public bool usarDirecaoInicial = false;
    public Direction direcaoFixaInicial;

    private List<Direction> conexoesAtuais = new List<Direction>();
    private RectTransform rectTransform;
    private int rotacaoAtual = 0;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (usarDirecaoInicial && conexoesOriginais.Count == 1)
        {
            Direction original = conexoesOriginais[0];
            int rotacoes = ((int)direcaoFixaInicial - (int)original + 4) % 4;
            rotacaoAtual = rotacoes * 90;
            rectTransform.localRotation = Quaternion.Euler(0, 0, -rotacaoAtual);
        }

        AtualizarConexoes();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!podeGirar) return;

        rotacaoAtual = (rotacaoAtual + 90) % 360;
        rectTransform.localRotation = Quaternion.Euler(0, 0, -rotacaoAtual);
        AtualizarConexoes();
    }

    public List<Direction> GetConexoes()
    {
        return conexoesAtuais;
    }

    private void AtualizarConexoes()
    {
        conexoesAtuais.Clear();
        foreach (var dir in conexoesOriginais)
        {
            var nova = DirecaoUtil.Rotacionada(dir, rotacaoAtual);
            conexoesAtuais.Add(nova);
        }
    }

    public int GetRotacaoAtual()
    {
        return rotacaoAtual;
    }

    public Vector2Int Posicao { get; set; }
    public bool IsStartTile => usarDirecaoInicial && podeGirar == false && conexoesOriginais.Count == 1;
    public bool IsEndTile => usarDirecaoInicial && podeGirar == false && conexoesOriginais.Count == 1;

}

public enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

