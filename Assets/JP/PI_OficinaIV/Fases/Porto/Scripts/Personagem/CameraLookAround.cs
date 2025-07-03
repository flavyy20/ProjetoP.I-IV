using UnityEngine;

public class CameraLookAround : MonoBehaviour
{
    public Transform alvo;         // Refer�ncia ao player (alvo a ser orbitado)
    public float distancia = 5f;   // Dist�ncia da c�mera ao alvo
    public float sensibilidadeX = 3f;
    public float sensibilidadeY = 2f;
    public float minY = -40f;
    public float maxY = 80f;

    private float rotX;
    private float rotY;

    void Start()
    {
        if (alvo == null)
        {
            Debug.LogError("A c�mera precisa de um alvo para seguir.");
        }

        Vector3 angulo = transform.eulerAngles;
        rotX = angulo.y;
        rotY = angulo.x;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(1)) // Bot�o direito
        {
            rotX += Input.GetAxis("Mouse X") * sensibilidadeX;
            rotY -= Input.GetAxis("Mouse Y") * sensibilidadeY;
            rotY = Mathf.Clamp(rotY, minY, maxY);

            Quaternion rotacao = Quaternion.Euler(rotY, rotX, 0);
            Vector3 posicao = rotacao * new Vector3(0.0f, 0.0f, -distancia) + alvo.position;

            transform.rotation = rotacao;
            transform.position = posicao;
        }
    }
}
