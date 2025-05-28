using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -4); // altura e dist�ncia atr�s do player

    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (player == null) return;

        // Rota��o da c�mera = rota��o do player
        Quaternion targetRotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

        // Posi��o da c�mera = posi��o do player + offset rotacionado para tr�s dele
        Vector3 desiredPosition = player.position + targetRotation * offset;

        // Move suavemente para a posi��o desejada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Rota��o da c�mera segue o player
       // transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
