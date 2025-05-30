using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPersonagem : MonoBehaviour
{
    public float velocidadeMovimento = 5f;
    public float sensibilidadeMouse = 100f;
    public Transform cameraPrincipal;

    private float rotacaoX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Esconde e trava o cursor
    }

    void Update()
    {
        // Movimento WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = transform.right * horizontal + transform.forward * vertical;
        transform.Translate(movimento * velocidadeMovimento * Time.deltaTime, Space.World);

        // Rotação com botão direito do mouse
        if (Input.GetMouseButton(1)) // Botão direito pressionado
        {
            float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

            rotacaoX -= mouseY;
            rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f); // Limita a rotação vertical

            cameraPrincipal.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
