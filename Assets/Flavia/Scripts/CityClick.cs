using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickCity : MonoBehaviour
{
    [Header("Cenas")]
    [SerializeField] private string nomeCenaDestino = "teste";        // Cena para onde vai ao clicar no cubo
    [SerializeField] private string nomeCenaVoltar = "TelaMonitoramento";   // Cena para voltar ao clicar no botão

    // 👉 Chamado quando o cubo é clicado
    private void OnMouseDown()
    {
        SceneManager.LoadScene("teste");
    }

    // 👉 Chamado quando o botão de voltar é pressionado
    public void VoltarParaCenaInicial()
    {
        SceneManager.LoadScene("TelaMonitoramento");
    }
}

