using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CityMonitor : MonoBehaviour
{
    public string cityName;
    public string sceneToLoad;
    public Slider progressBar;
    public int indexCity;

    private float progressSpeed;
    private float currentProgress;
    private Image fillImage;

    public float Progress => currentProgress;

    private bool isDemolished = false;
    private Image cityImage;

    void Start()
    {
        fillImage = progressBar.fillRect.GetComponent<Image>();
        cityImage = GetComponent<Image>();
        UpdateFillColor();
        SaveCity(indexCity);
    }

    void Update()
    {
        if (currentProgress < 1f)
        {
            currentProgress += Time.deltaTime * progressSpeed;
            progressBar.value = currentProgress;
            UpdateFillColor();
        }
        else if (!isDemolished)
        {
            OnCityDemolished();
        }
    }

    public void SetSpeed(float speed)
    {
        progressSpeed = speed;
    }

    void UpdateFillColor()
    {
        if (fillImage == null) return;

        if (currentProgress <= 0.5f)
            fillImage.color = Color.green;
        else if (currentProgress <= 0.8f)
            fillImage.color = Color.yellow;
        else
            fillImage.color = Color.red;
    }

    void OnCityDemolished()
    {
        isDemolished = true;

        if (cityImage != null)
        {
            cityImage.color = Color.red;
        }

        Debug.Log($"Cidade {cityName} foi demolida!");
    }

    public void OnCityClick()
    {
        if (isDemolished)
        {
            Debug.Log($"Cidade {cityName} já foi demolida. Não é possível acessar a cena.");
            return;
        }
        SaveCity(indexCity);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void SaveCity(int numeroCena)
    {
        SaveData dados = FindObjectOfType<ControlarJogo>()._save.CarregarSave() ?? new SaveData(4);
        dados.fases[numeroCena]._progressao = currentProgress;
        dados.fases[numeroCena]._finalizado = false;
        FindObjectOfType<ControlarJogo>()._save.SalvarJogo(dados);
    }
}
