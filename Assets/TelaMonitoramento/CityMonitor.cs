using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CityMonitor : MonoBehaviour
{
    public string cityName;
    public string sceneToLoad;
    public Slider progressBar;

    private float progressSpeed = 0f;
    private float currentProgress = 0f;
    private Image fillImage;

    void Start()
    {
        fillImage = progressBar.fillRect.GetComponent<Image>();
        UpdateFillColor();
    }

    void Update()
    {
        if (currentProgress < 1f)
        {
            currentProgress += Time.deltaTime * progressSpeed;
            progressBar.value = currentProgress;
            UpdateFillColor();
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

    public void OnCityClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
