using UnityEngine;

public class CityAlertVisual : MonoBehaviour
{
    public Canvas canvas;
    public GameObject fundoAlerta;
    public RectTransform[] spawnAlertaPoints;
    public CityManager cityManager;

    private bool[] alertaAtivado;

    void Start()
    {
        fundoAlerta.SetActive(false);
        alertaAtivado = new bool[cityManager.cities.Count];
    }

    void Update()
    {
        for (int i = 0; i < cityManager.cities.Count; i++)
        {
            float progress = GetProgressFromCity(i);

            if (!alertaAtivado[i] && GetLevel(progress) == CityManager.Level.Amarelo)
            {
                AtivarAlerta(i);
            }
        }
    }

    void AtivarAlerta(int cityIndex)
    {
        alertaAtivado[cityIndex] = true;

        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, spawnAlertaPoints[cityIndex].position);

        RectTransform fundoRect = fundoAlerta.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(), screenPos, null, out Vector2 localPoint);

        fundoRect.anchoredPosition = localPoint;
        fundoAlerta.SetActive(true);

        Debug.Log($" Alerta ativado para cidade {cityIndex}!");
    }

    CityManager.Level GetLevel(float progress)
    {
        if (progress >= 0.8f)
            return CityManager.Level.Vermelho;
        else if (progress >= 0.5f)
            return CityManager.Level.Amarelo;
        else
            return CityManager.Level.Verde;
    }

    float GetProgressFromCity(int cityIndex)
    {
        return cityManager.GetProgress(cityIndex);
    }
}
