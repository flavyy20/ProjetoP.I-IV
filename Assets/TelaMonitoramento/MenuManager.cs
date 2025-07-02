using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject painelConfiguracoes;
    public GameObject painelCreditos;
    public GameObject painelCutscene;

    public VideoPlayer cutscenePlayer;
    private bool cutscenePausada = false;

    public void AbrirConfiguracoes()
    {
        painelConfiguracoes.SetActive(true);
    }

    public void FecharConfiguracoes()
    {
        painelConfiguracoes.SetActive(false);
    }

    public void AbrirCreditos()
    {
        painelCreditos.SetActive(true);
    }

    public void FecharCreditos()
    {
        painelCreditos.SetActive(false);
    }

    public void Jogar()
    {
        StartCoroutine(TocarCutscene());
    }

    private IEnumerator TocarCutscene()
    {
        cutscenePlayer.Prepare();

        while (!cutscenePlayer.isPrepared)
            yield return null;

        cutscenePlayer.time = 0; // garante início no tempo 0
        cutscenePlayer.Play();

        yield return new WaitForSeconds(0.1f); // pequeno delay antes de exibir
        painelCutscene.SetActive(true);

        cutscenePlayer.loopPointReached += OnCutsceneTerminou;
    }

    private void OnCutsceneTerminou(VideoPlayer vp)
    {
        SceneManager.LoadScene("TelaMonitoramento");
    }
    public void AlternarPauseCutscene()
    {
        if (cutscenePlayer == null || !cutscenePlayer.isPrepared)
            return;

        if (cutscenePausada)
        {
            cutscenePlayer.Play();
            cutscenePausada = false;
        }
        else
        {
            cutscenePlayer.Pause();
            cutscenePausada = true;
        }
    }
}
