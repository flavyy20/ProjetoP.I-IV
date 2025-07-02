using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TuboUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public string idTubo;
    private RectTransform rectTransform;
    private Vector2 posicaoOriginal;
    private Image imagemTubo;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        imagemTubo = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        posicaoOriginal = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        imagemTubo.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        imagemTubo.raycastTarget = true;
        VerificarEncaixe();
    }

    void VerificarEncaixe()
    {
        foreach (Conector conector in PuzzleManager.instance.conectores)
        {
            if (conector.idTubo == this.idTubo)
            {
                float distancia = Vector2.Distance(
                    rectTransform.anchoredPosition,
                    conector.GetComponent<RectTransform>().anchoredPosition);

                if (distancia < PuzzleManager.instance.distanciaEncaixe)
                {
                    rectTransform.anchoredPosition =
                        conector.GetComponent<RectTransform>().anchoredPosition;
                    imagemTubo.color = Color.green;
                    conector.estaOcupado = true;

                    PuzzleManager.instance.VerificarCompleto();
                    return;
                }
            }
        }

        // Se n�o encaixou, volta para a posi��o original
        rectTransform.anchoredPosition = posicaoOriginal;
        imagemTubo.color = Color.red;
    }

    public void ResetarPosicao()
    {
        rectTransform.anchoredPosition = posicaoOriginal;
        imagemTubo.color = Color.white;
    }
}