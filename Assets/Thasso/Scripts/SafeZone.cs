using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se � um NPC
        if (other.CompareTag("NPC"))
        {
            Debug.Log("V�tima " + other.gameObject.name + " est� segura!");
            // Para o movimento
            NPCRandomWalk npc = other.GetComponent<NPCRandomWalk>();
            if (npc != null)
            {
                npc.StopMovement(); // M�todo que vamos garantir que exista
            }
        }
    }
}
