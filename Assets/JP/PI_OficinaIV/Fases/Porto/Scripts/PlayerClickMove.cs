using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPorClique : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string plataformaTag = "Plataforma";

    private Collider playerCollider;
    private int npcsColetados = 0;
    private const int npcsNecessarios = 2;

    void Start()
    {
        playerCollider = GetComponent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogError("Este objeto precisa de um Collider para calcular a altura corretamente.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(plataformaTag))
                {
                    float altura = playerCollider != null ? playerCollider.bounds.extents.y : 0f;
                    Vector3 destino = hit.point + Vector3.up * altura;
                    transform.position = destino;
                }
                else
                {
                    Debug.Log("Você clicou fora de uma plataforma.");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            // Parenteia o NPC ao player
            other.transform.SetParent(transform);

            // Reposiciona do lado do player
            Vector3 offset = new Vector3(1.2f * (npcsColetados + 1), 0f, 0f);
            other.transform.localPosition = offset;

            // Desativa colisão para evitar reentradas
            Collider npcCol = other.GetComponent<Collider>();
            if (npcCol != null) npcCol.enabled = false;

            npcsColetados++;
            Debug.Log("NPC coletado: " + npcsColetados);

            if (npcsColetados >= npcsNecessarios)
            {
                SceneManager.LoadScene("TelaMonitoramento");
            }
        }
    }
}
