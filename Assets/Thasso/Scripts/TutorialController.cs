using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject resgateTexto;
    public Button skipButton;

    void Start()
    {
        Time.timeScale = 0f;

        tutorialPanel.SetActive(true);

        skipButton.onClick.AddListener(FecharTutorial);
    }

    public void FecharTutorial()
    {
        Time.timeScale = 1f; 
        tutorialPanel.SetActive(false);
        resgateTexto.SetActive(true); 
    }
}
