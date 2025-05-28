using UnityEngine;

[ExecuteAlways]
public class CameraAjuste : MonoBehaviour
{
    public Camera cam;
    public float horizontalFOV = 60f; // Ajuste esse valor como quiser

    void Update()
    {
        if (cam == null)
            cam = GetComponent<Camera>();

        // Converte FOV horizontal para vertical baseado no aspecto da tela
        float aspect = cam.aspect;
        float verticalFOV = 2f * Mathf.Atan(Mathf.Tan(horizontalFOV * Mathf.Deg2Rad / 2f) / aspect) * Mathf.Rad2Deg;

        cam.fieldOfView = verticalFOV;
    }
}
