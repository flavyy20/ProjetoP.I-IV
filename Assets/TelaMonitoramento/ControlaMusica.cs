using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ControlaMusica : MonoBehaviour
{
    [Header("M�sica que toca no menu")]
    public AudioSource musicaMenu;

    [Header("M�sica que toca durante a cutscene")]
    public AudioSource musicaCutscene;

    [Header("Video Player da cutscene")]
    public VideoPlayer video;

    [Header("Nome da pr�xima cena")]
    public string proximaCena;

    void Start()
    {
        video.started += OnVideoStarted;
        video.loopPointReached += OnVideoEnded; // Quando o v�deo terminar
    }

    void OnVideoStarted(VideoPlayer vp)
    {
        if (musicaMenu != null && musicaMenu.isPlaying)
        {
            musicaMenu.Stop();
        }

        if (musicaCutscene != null && !musicaCutscene.isPlaying)
        {
            musicaCutscene.Play();
        }
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        if (musicaCutscene != null && musicaCutscene.isPlaying)
        {
            musicaCutscene.Stop();
        }

        // Carrega a pr�xima cena (defina o nome no Inspector)
        SceneManager.LoadScene("TelaMonitoramento");
    }
}
