using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerCidades : MonoBehaviour
{
    public static GameManagerCidades Instance;
    public CityState[] cities;  // Lista de cidades
    public Slider[] progressBars;  // Referências aos sliders na tela inicial
    public Text globalAlertText, cityAlertText;  // Texto para exibir alertas
    public float velBarra;

    [System.Serializable]
    public class CityState
    {
        public string cityName;  // Nome da cidade
        public float progress;   // Progresso da barra (0 a 1)
        public bool isEventActive;  // Evento aleatório ativo
    }

   

    [Header("Configuração de Eventos")]
    public float minEventDelay = 5f;  // Tempo mínimo para próximo evento
    public float maxEventDelay = 15f; // Tempo máximo para próximo evento
    private string currentCity = null;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
                     
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        foreach (var city in cities)
        {
            StartCoroutine(ScheduleNextEvent(city)); // Inicia eventos para cada cidade
        }
    }

    private void Update()
    {
       
       
        UpdateCityProgress();  // Atualiza progresso das cidades
        UpdateProgressBars(); // Atualiza sliders na tela inicial
    }

    private void UpdateCityProgress()
    {
        foreach (var city in cities)
        {
            if (city.isEventActive)
            {
                city.progress += Time.deltaTime * velBarra; // Ajuste a velocidade de progresso
                if (city.progress >= 1f)
                {
                    city.progress = 1f;
                    TriggerGlobalAlert($"{city.cityName}: Resolva o evento!");
                }
            }
        }
    }

    private void UpdateProgressBars()
    {
        for (int i = 0; i < cities.Length; i++)
        {
            progressBars[i].value = cities[i].progress;
        }

        //if (progressBars == null || progressBars.Length != cities.Length) return;

        //for (int i = 0; i < cities.Length; i++)
        //{
        //    if (progressBars[i] != null)
        //    {
        //        progressBars[i].value = cities[i].progress;
        //    }
        //}

    }

    private void UpdateProgressBarsReferences()
    {
        // Só procura os sliders se estiver na cena inicial
        if (SceneManager.GetActiveScene().name == "TelaIInicio")
        {
            // Encontra o objeto pai dos sliders (ajuste o nome conforme sua hierarquia)
            GameObject progressBarsParent = GameObject.Find("ProgressBarsContainer");

            if (progressBarsParent != null)
            {
                progressBars = progressBarsParent.GetComponentsInChildren<Slider>(true);

                // Verifica se encontrou a quantidade certa de sliders
                if (progressBars.Length != cities.Length)
                {
                    Debug.LogError($"Encontrados {progressBars.Length} sliders, mas precisamos de {cities.Length}");
                    progressBars = null;
                }
            }
            else
            {
                Debug.LogError("Container dos ProgressBars não encontrado na cena!");
                progressBars = null;
            }
        }
        else
        {
            progressBars = null;
        }
    }

    private IEnumerator ScheduleNextEvent(CityState city)
    {
        while (true)
        {
            if (!city.isEventActive)
            {
                float delay = Random.Range(minEventDelay, maxEventDelay);
                yield return new WaitForSeconds(delay);

                city.isEventActive = true;
                TriggerGlobalAlert($"{city.cityName}: Evento iniciado!");
                if (currentCity != city.cityName)
                {
                    TriggerCityAlert($"{city.cityName}: Um novo evento foi ativado!");
                }
            }
            else
            {
                yield return null; // Aguarda enquanto o evento está ativo
            }
        }
    }

    public void ResolveEvent(string cityName)
    {
        foreach (var city in cities)
        {
            if (city.cityName == cityName)
            {
                city.isEventActive = false;
                city.progress = 0f; // Reinicia o progresso após resolver
                TriggerGlobalAlert($"{city.cityName}: Evento resolvido!");
                break;
            }
        }
    }

    public void EnterCity(string cityName)
    {
        currentCity = cityName; // Atualiza a cidade atual
        UnityEngine.SceneManagement.SceneManager.LoadScene(cityName);
    }

    public void ReturnToMain()
    {
        currentCity = null; // Volta para a tela inicial
        UnityEngine.SceneManagement.SceneManager.LoadScene("TelaIInicio");
    }
    public void TriggerGlobalAlert(string message)
    {
        globalAlertText.text = message;
        Invoke(nameof(ClearAlert), 5f); // Limpa o alerta após 5 segundos
    }
    private void ClearGlobalAlert()
    {
        if (globalAlertText != null)
        {
            globalAlertText.text = "";
        }
    }
    public void TriggerCityAlert(string message)
    {
        if (cityAlertText != null)
        {
            cityAlertText.text = message;
            Invoke(nameof(ClearCityAlert), 5f); // Limpa o alerta após 5 segundos
        }
    }
    private void ClearCityAlert()
    {
        if (cityAlertText != null)
        {
            cityAlertText.text = "";
        }
    }
    private void ClearAlert()
    {
        globalAlertText.text = "";
    }




    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Atualiza referências ao carregar uma nova cena
        UpdateCityAlertReference();
        UpdateGlobalAlert();
        //UpdateProgressBars();
        UpdateProgressBarsReferences();
    }

    private void UpdateCityAlertReference()
    {
        // Procura pelo objeto "CityAlertText" na nova cena
        GameObject cityAlertObject = GameObject.Find("CityAlertText");
        if (cityAlertObject != null)
        {
            cityAlertText = cityAlertObject.GetComponent<Text>();
        }
        else
        {
            Debug.LogWarning("CityAlertText não encontrado na cena atual.");
            cityAlertText = null; // Evita erros de referência
        }
    }

    private void UpdateGlobalAlert()
    {
        // Procura pelo objeto "GlobalTextAlert" na nova cena
        GameObject globalAlertObject = GameObject.Find("GlobalTextAlert");
        if (globalAlertObject != null)
        {
            globalAlertText = globalAlertObject.GetComponent<Text>();
        }
        else
        {
            Debug.LogWarning("GlobalTextAlert não encontrado na cena atual.");
            globalAlertText = null; // Evita erros de referência
        }
    }
}
