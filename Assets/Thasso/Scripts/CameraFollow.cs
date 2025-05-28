using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -4); // altura e distância atrás do player

    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (player == null) return;

        // Rotação da câmera = rotação do player
        Quaternion targetRotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

        // Posição da câmera = posição do player + offset rotacionado para trás dele
        Vector3 desiredPosition = player.position + targetRotation * offset;

        // Move suavemente para a posição desejada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Rotação da câmera segue o player
       // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
