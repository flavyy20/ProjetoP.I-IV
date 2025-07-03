using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlarJogoECO : MonoBehaviour
{
    public static ControlarJogoECO Instance { get; private set; }
    public Save _save;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _save = GetComponent<Save>();
    }

    public void Jogar()
    {
        if (_save.VerificarSave())
        {
            SaveDataECO dados = _save.CarregarSave();
            for (int i = 0; i < dados.fases.Length; i++)
            {
                if (dados.fases[i].faseAtual)
                {
                    SceneManager.LoadScene(i);
                    return;
                }
            }
        }
        // Se não encontrou save ou fase atual, começa do início
        SceneManager.LoadScene(0);
    }

    public FaseECO[] ObterHistoricoFases()
    {
        if (_save.VerificarSave())
        {
            return _save.CarregarSave().fases;
        }
        return null;
    }
}
