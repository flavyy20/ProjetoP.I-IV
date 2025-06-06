using UnityEngine;

public class NPCRescue : MonoBehaviour
{
    public float rescueRange = 2f; // raio de alcance para "resgatar"
    public KeyCode rescueKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(rescueKey))
        {
            Debug.Log("Espaço pressionado: tentando resgatar NPC...");
            TryRescueNPC();
        }
    }

    void TryRescueNPC()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, rescueRange);
        Debug.Log($"NPCRescue encontrou {hits.Length} objetos no raio {rescueRange}");

        foreach (var hit in hits)
        {
            Debug.Log($"Verificando {hit.name}");

            // Sobe a hierarquia procurando o NPCRandomWalk
            Transform current = hit.transform;
            while (current != null)
            {
                NPCRandomWalk npc = current.GetComponent<NPCRandomWalk>();
                if (npc != null)
                {
                    Debug.Log($"Encontrado NPC válido: {npc.name}, isCaught = {npc.IsCaught()}");

                    if (npc.IsCaught())
                    {
                        npc.AttachToPlayer(transform);
                        Debug.Log($"Resgatando NPC {npc.name}");
                        return;
                    }
                }
                current = current.parent;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rescueRange);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Collider[] hits = Physics.OverlapSphere(transform.position, rescueRange);
        foreach (var hit in hits)
        {
            Gizmos.DrawLine(transform.position, hit.transform.position);
        }
    }
}
