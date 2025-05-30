using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrasicaoCena : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnCenaCarregada;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnCenaCarregada;
    }

    void OnCenaCarregada(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == GameM.instance.cenaPosDesastre)
        {
            foreach (PontoDeFuga ponto in FindObjectsOfType<PontoDeFuga>())
            {
                ponto.SpawnarNPCsPosDesastre(GameM.instance.npcsParaSpawnar);
            }
        }
    }
}
