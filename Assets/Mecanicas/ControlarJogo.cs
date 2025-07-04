using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ControlarJogo : MonoBehaviour
{
    public static ControlarJogo Instance { get; private set; }
    public Save _save;

    private void Awake()
    {
        if(Instance == null)
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jogar()
    {
        if(_save.VerificarSave())
        {
            SaveData dados = _save.CarregarSave();
            for(int i = 0; i > dados.fases.Length; i++)
            {
                if (dados != null && dados.progressao._faseAtual)
                {
                    SceneManager.LoadScene(i);
                }
            }
        }
    }
}
