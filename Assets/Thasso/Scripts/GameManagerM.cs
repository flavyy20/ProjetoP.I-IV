using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerM : MonoBehaviour
{
    public static GameManagerM Instance;

    public TMP_Text textoResgates; // arraste o Text "Resgates: 0/4"
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
        textoResgates.text = $"Resgates: {totalResgates}/{maxResgates}";
    }
}
