using UnityEngine;
using System.Collections;  // Adiciona essa linha

public class CarLoopMove : MonoBehaviour
{
    public float speed = 10f;
    public Transform pointA;
    public Transform pointB;
    public float teleportDelay = 2f; // Atraso de 2 segundos antes do teleporte

    private Vector3 previousPosition;
    private bool isTeleporting = false;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        // Movimento reto para frente
        transform.position += transform.forward * speed * Time.deltaTime;

        // Verifica se ultrapassou o ponto A
        Vector3 toA = pointA.position - previousPosition;
        Vector3 toNow = transform.position - previousPosition;

        if (Vector3.Dot(toA, toNow) < 0 && !isTeleporting)
        {
            // Inicia a corrotina para teleporte após o atraso
            StartCoroutine(TeleportAfterDelay());
        }

        previousPosition = transform.position;
    }

    // Corrotina para teleporte após o atraso
    private IEnumerator TeleportAfterDelay()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(teleportDelay); // Espera pelo atraso

        // Teleporta, mantendo o Y original
        transform.position = new Vector3(pointB.position.x, transform.position.y, pointB.position.z);

        // Permite teleportar novamente após a próxima ultrapassagem
        isTeleporting = false;
    }
}
