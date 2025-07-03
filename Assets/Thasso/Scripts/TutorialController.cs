using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject painelResgates; // novo campo: painel com TMP_Text dentro
    public Button skipButton;

    void Start()
    {
        Time.timeScale = 0f;

        tutorialPanel.SetActive(true);

        skipButton.onClick.AddListener(FecharTutorial);

        // Garante que o painel de resgate está desativado no início
        if (painelResgates != null)
        {
            painelResgates.SetActive(false);
        }
    }

    public void FecharTutorial()
    {
        Time.timeScale = 1f;
        tutorialPanel.SetActive(false);

        if (painelResgates != null)
        {
            painelResgates.SetActive(true); // ativa painel com fundo e texto
        }
    }
}
