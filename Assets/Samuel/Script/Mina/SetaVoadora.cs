using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetaVoadora : MonoBehaviour
{

    [Header("Refer�ncias")]
    public Transform player;              // Personagem do jogador
    public Transform target;             // Objetivo a ser alcan�ado
    public Camera isoCamera;             // C�mera isom�trica

    [Header("Configura��es")]
    [Range(0.05f, 0.4f)]
    public float edgeMargin = 0.1f;      // Margem da borda da tela
    public float heightOffset = 2f;      // Altura da seta em rela��o ao ch�o
    public float smoothSpeed = 5f;       // Suaviza��o do movimento

    [Header("Feedback Visual")]
    public bool showDistance = true;
    public TextMeshPro distanceText;
    public float minVisibleDistance = 5f;

    private Vector3 targetScreenPos;
    private bool isTargetVisible;
    private Renderer arrowRenderer;

    [Header("Configura��es de Zona Segura")]
    public float distanciaAtivacao = 50f; // Dist�ncia para ativar automaticamente
    public float distanciaDesativacao = 5f; // Dist�ncia para considerar "chegou"

    [Header("Configura��o Din�mica")]
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


        // Verifica dist�ncia para ativar/desativar automaticamente
        float distancia = Vector3.Distance(player.position, target.position);

        if (distancia < distanciaDesativacao)
        {
            gameObject.SetActive(false);
            return;
        }

        // Calcula a posi��o do objetivo na tela
        targetScreenPos = isoCamera.WorldToViewportPoint(target.position);

        // Verifica se o objetivo est� vis�vel na c�mera
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

        //// Calcula a posi��o do objetivo na tela
        //targetScreenPos = isoCamera.WorldToViewportPoint(target.position);

        //// Verifica se o objetivo est� vis�vel na c�mera
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
        // Calcula a dire��o do centro para o objetivo
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0);
        Vector3 screenDir = (targetScreenPos - screenCenter).normalized;

        // Calcula a posi��o na borda da tela
        float screenAspect = (float)Screen.width / Screen.height;
        float edgeX = Mathf.Clamp(targetScreenPos.x, edgeMargin, 1 - edgeMargin);
        float edgeY = Mathf.Clamp(targetScreenPos.y, edgeMargin, 1 - edgeMargin);

        // Se o objetivo estiver atr�s da c�mera, inverte a dire��o
        if (targetScreenPos.z < 0)
        {
            screenDir *= -1;
        }

        // Encontra o ponto de intersec��o com as bordas
        float xRatio = Mathf.Abs((0.5f - edgeX) / screenDir.x);
        float yRatio = Mathf.Abs((0.5f - edgeY) / screenDir.y);
        float minRatio = Mathf.Min(xRatio, yRatio);

        Vector3 edgeViewportPos = new Vector3(
            0.5f + screenDir.x * minRatio,
            0.5f + screenDir.y * minRatio,
            Mathf.Max(targetScreenPos.z, 0.1f)); // Garante Z positivo

        // Converte para posi��o mundial com altura fixa
        Vector3 worldPos = isoCamera.ViewportToWorldPoint(edgeViewportPos);
        worldPos.y = player.position.y + heightOffset;

        // Suaviza o movimento
        transform.position = Vector3.Lerp(transform.position, worldPos, smoothSpeed * Time.deltaTime);
    }

    void UpdateArrowRotation()
    {
        // Calcula a dire��o para o objetivo (ignorando altura)
        Vector3 flatTargetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 direction = flatTargetPos - transform.position;

        // Rotaciona a seta para olhar para o objetivo
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }

        // Ajuste adicional para c�mera isom�trica
        transform.rotation *= Quaternion.Euler(10f, 0, 38f);
    }

    void UpdateDistanceText()
    {
        if (!showDistance || distanceText == null) return;

        float distance = Vector3.Distance(player.position, target.position);
        distanceText.text = Mathf.RoundToInt(distance) + "m";

        // Mant�m o texto leg�vel na perspectiva isom�trica
        distanceText.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        distanceText.transform.position = transform.position + Vector3.up * 1.5f;
    }

   
}
