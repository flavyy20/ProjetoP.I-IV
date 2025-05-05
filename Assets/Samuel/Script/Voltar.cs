using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Voltar : MonoBehaviour
{
    public Button backButton, minaBT, bacoBT;

    private void Start()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() =>
            {
                if (GameManagerCidades.Instance != null)
                {
                    GameManagerCidades.Instance.ReturnToMain();
                }
                else
                {
                    Debug.LogError("GameManagerCidades não está acessível!");
                }
            });
        }


        if (minaBT != null)
        {
            minaBT.onClick.RemoveAllListeners();
            minaBT.onClick.AddListener(() =>
            {
                if (GameManagerCidades.Instance != null)
                {
                    GameManagerCidades.Instance.EnterCity("Mina");
                }
                else
                {
                    Debug.LogError("GameManagerCidades não está acessível!");
                }
            });
        }

        if (bacoBT != null)
        {
            bacoBT.onClick.RemoveAllListeners();
            bacoBT.onClick.AddListener(() =>
            {
                if (GameManagerCidades.Instance != null)
                {
                    GameManagerCidades.Instance.EnterCity("Baco");
                }
                else
                {
                    Debug.LogError("GameManagerCidades não está acessível!");
                }
            });
        }
    }

}
