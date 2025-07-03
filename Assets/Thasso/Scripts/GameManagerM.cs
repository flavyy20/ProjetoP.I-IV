using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerM : MonoBehaviour
{
    public static GameManagerM Instance;

    public GameObject painelResgates; // arraste aqui o GameObject da UI Image (pai do TMP_Text)

    private int totalResgates = 0;
    public int maxResgates = 4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarResgate()
    {
        totalResgates++;

        // Atualiza o texto dentro do painel
        TMP_Text texto = painelResgates.GetComponentInChildren<TMP_Text>();
        if (texto != null)
        {
            texto.text = $"Resgates: {totalResgates}/{maxResgates}";
        }

        // Exemplo: mostrar o painel (se estiver escondido)
        painelResgates.SetActive(true);
    }

    public void AtivarPainel(bool ativo)
    {
        painelResgates.SetActive(ativo);
    }
}
