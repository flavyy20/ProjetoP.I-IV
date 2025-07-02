using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetaVoadora : MonoBehaviour
{

    [Header("Referências")]
    public Transform player;              // Personagem do jogador
    public Transform target;             // Objetivo a ser alcançado
    public Camera isoCamera;             // Câmera isométrica

    [Header("Configurações")]
    [Range(0.05f, 0.4f)]
    public float edgeMargin = 0.1f;      // Margem da borda da tela
    public float heightOffset = 2f;      // Altura da seta em relação ao chão
    public float smoothSpeed = 5f;       // Suavização do movimento

    [Header("Feedback Visual")]
    public bool showDistance = true;
    public TextMeshPro distanceText;
    public float minVisibleDistance = 5f;

    private Vector3 targetScreenPos;
    private bool isTargetVisible;
    private Renderer arrowRenderer;

    [Header("Configurações de Zona Segura")]
    public float distanciaAtivacao = 50f; // Distância para ativar automaticamente
    public float distanciaDesativacao = 5f; // Distância para considerar "chegou"

    [Header("Configuração Dinâmica")]
    public bool alvoAtivo = true;
    void Start()
    {
        arrowRenderer = GetComponent<Renderer>();
        if (isoCamera == null) isoCamera = Camera.main;

        // Garante que a seta comece oculta
        arrowRenderer.enabled = false;
    }

    void LateUpdate()
    {

        //if (player == null || target == null || isoCamera == null) return;
        if (!alvoAtivo || player == null || target == null || isoCamera == null)
        {
            arrowRenderer.enabled = false;
            return;
        }


        // Verifica distância para ativar/desativar automaticamente
        float distancia = Vector3.Distance(player.position, target.position);

        if (distancia < distanciaDesativacao)
        {
            gameObject.SetActive(false);
            return;
        }

        // Calcula a posição do objetivo na tela
        targetScreenPos = isoCamera.WorldToViewportPoint(target.position);

        // Verifica se o objetivo está visível na câmera
        isTargetVisible = IsTargetOnScreen(targetScreenPos);

        // Ativa/desativa a seta conforme necessidade
        arrowRenderer.enabled = (!isTargetVisible && distancia > minVisibleDistance) ||
                              (distancia < distanciaAtivacao && distancia > distanciaDesativacao);

        if (arrowRenderer.enabled)
        {
            UpdateArrowPosition();
            UpdateArrowRotation();
            UpdateDistanceText();
        }
        //if (player == null || target == null || isoCamera == null) return;

        //// Calcula a posição do objetivo na tela
        //targetScreenPos = isoCamera.WorldToViewportPoint(target.position);

        //// Verifica se o objetivo está visível na câmera
        //isTargetVisible = IsTargetOnScreen(targetScreenPos);

        //// Ativa/desativa a seta conforme necessidade
        //arrowRenderer.enabled = !isTargetVisible &&
        //                      Vector3.Distance(player.position, target.position) > minVisibleDistance;

        //if (arrowRenderer.enabled)
        //{
        //    UpdateArrowPosition();
        //    UpdateArrowRotation();
        //    UpdateDistanceText();
        //}
    }

    public void ConfigurarAlvo(Transform novoAlvo)
    {
        target = novoAlvo;
        alvoAtivo = (novoAlvo != null);
    }
    bool IsTargetOnScreen(Vector3 viewportPos)
    {
        return viewportPos.z > 0 &&
               viewportPos.x > edgeMargin && viewportPos.x < 1 - edgeMargin &&
               viewportPos.y > edgeMargin && viewportPos.y < 1 - edgeMargin;
    }

    void UpdateArrowPosition()
    {
        // Calcula a direção do centro para o objetivo
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0);
        Vector3 screenDir = (targetScreenPos - screenCenter).normalized;

        // Calcula a posição na borda da tela
        float screenAspect = (float)Screen.width / Screen.height;
        float edgeX = Mathf.Clamp(targetScreenPos.x, edgeMargin, 1 - edgeMargin);
        float edgeY = Mathf.Clamp(targetScreenPos.y, edgeMargin, 1 - edgeMargin);

        // Se o objetivo estiver atrás da câmera, inverte a direção
        if (targetScreenPos.z < 0)
        {
            screenDir *= -1;
        }

        // Encontra o ponto de intersecção com as bordas
        float xRatio = Mathf.Abs((0.5f - edgeX) / screenDir.x);
        float yRatio = Mathf.Abs((0.5f - edgeY) / screenDir.y);
        float minRatio = Mathf.Min(xRatio, yRatio);

        Vector3 edgeViewportPos = new Vector3(
            0.5f + screenDir.x * minRatio,
            0.5f + screenDir.y * minRatio,
            Mathf.Max(targetScreenPos.z, 0.1f)); // Garante Z positivo

        // Converte para posição mundial com altura fixa
        Vector3 worldPos = isoCamera.ViewportToWorldPoint(edgeViewportPos);
        worldPos.y = player.position.y + heightOffset;

        // Suaviza o movimento
        transform.position = Vector3.Lerp(transform.position, worldPos, smoothSpeed * Time.deltaTime);
    }

    void UpdateArrowRotation()
    {
        // Calcula a direção para o objetivo (ignorando altura)
        Vector3 flatTargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 direction = flatTargetPos - transform.position;

        // Rotaciona a seta para olhar para o objetivo
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }

        // Ajuste adicional para câmera isométrica
        transform.rotation *= Quaternion.Euler(10f, 0, 38f);
    }

    void UpdateDistanceText()
    {
        if (!showDistance || distanceText == null) return;

        float distance = Vector3.Distance(player.position, target.position);
        distanceText.text = Mathf.RoundToInt(distance) + "m";

        // Mantém o texto legível na perspectiva isométrica
        distanceText.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        distanceText.transform.position = transform.position + Vector3.up * 1.5f;
    }

   
}
