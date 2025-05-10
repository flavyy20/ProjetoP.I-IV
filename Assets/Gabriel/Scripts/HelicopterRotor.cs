using UnityEngine;

public class HelicopterRotor : MonoBehaviour
{
    [Header("Configuração da Hélice")]
    public float maxRotationSpeed = 1000f; // Velocidade máxima
    public Vector3 rotationAxis = Vector3.up; // Eixo da hélice

    [Header("Tempo de resposta")]
    public float accelerationTime = 2f; // Tempo para atingir velocidade máxima

    private float currentSpeed = 0f; // Velocidade atual
    public bool engineOn = false; // Estado do motor

    void Update()
    {
        // Acelera ou desacelera suavemente
        float targetSpeed = engineOn ? maxRotationSpeed : 0f;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);

        // Rotaciona a hélice
        transform.Rotate(currentSpeed * Time.deltaTime * rotationAxis);
    }
}
