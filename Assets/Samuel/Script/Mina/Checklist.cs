using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checklist : MonoBehaviour
{
    public GameObject checklistPanel;
    public Text problemasText;
    public Toggle[] solucoesToggle;

    private Barragem barragem;

    void Start()
    {
        checklistPanel.SetActive(false);
        barragem = FindObjectOfType<Barragem>();
    }

    public void MostrarChecklist()
    {
        //problemasText.text = barragem.GetDescricaoProblemas();
        checklistPanel.SetActive(true);
        Time.timeScale = 0; // Pausa o jogo
    }

    public void FecharChecklist()
    {
        checklistPanel.SetActive(false);
        Time.timeScale = 1; // Despausa o jogo
    }

    public void VerificarSolucao()
    {
        bool todosResolvidos = true;
        foreach (Toggle toggle in solucoesToggle)
        {
            if (!toggle.isOn) todosResolvidos = false;
        }

        if (todosResolvidos)
        {
            // Todos os problemas resolvidos
            FecharChecklist();
            Debug.Log("Barragem reparada com sucesso!");
        }
    }
}
