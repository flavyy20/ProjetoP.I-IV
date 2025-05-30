using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Referências")]
    public CamSeguir camSeguir;
    public float tempoTransicao = 1f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void FocarPuzzle(Transform pontoFoco)
    {
        camSeguir.PararControle();
        StartCoroutine(MoverCamera(pontoFoco));
    }

    public void VoltarAoJogador()
    {
        camSeguir.RetomarControle();
    }

    private IEnumerator MoverCamera(Transform alvo)
    {
        float tempo = 0;
        Vector3 posicaoInicial = camSeguir.transform.position;
        Quaternion rotacaoInicial = camSeguir.transform.rotation;

        while (tempo < tempoTransicao)
        {
            camSeguir.transform.position = Vector3.Lerp(
                posicaoInicial,
                alvo.position,
                tempo / tempoTransicao
            );

            camSeguir.transform.rotation = Quaternion.Lerp(
                rotacaoInicial,
                Quaternion.LookRotation(alvo.forward),
                tempo / tempoTransicao
            );

            tempo += Time.deltaTime;
            yield return null;
        }
    }
}
