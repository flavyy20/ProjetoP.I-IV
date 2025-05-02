using UnityEngine;

public class HelicopterRotor : MonoBehaviour
{
    [Header("Configura��o da H�lice")]
    public float maxRotationSpeed = 1000f; // Velocidade m�xima
    public Vector3 rotationAxis = Vector3.up; // Eixo da h�lice

    [Header("Tempo de resposta")]
    public float accelerationTime = 2f; // Tempo para atingir velocidade m�xima

    private float currentSpeed = 0f; // Velocidade atual
    public bool engineOn = false; // Estado do motor

    void Update()
    {
        // Alternar motor com a tecla E (ou troque por seu sistema de input)
        if (Input.GetKeyDown(KeyCode.E))
        {
            engineOn = !engineOn;
        }

        // Acelera ou desacelera suavemente
        float targetSpeed = engineOn ? maxRotationSpeed : 0f;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / accelerationTime);

        // Rotaciona a h�lice
        transform.Rotate(rotationAxis * currentSpeed * Time.deltaTime);
    }
}
