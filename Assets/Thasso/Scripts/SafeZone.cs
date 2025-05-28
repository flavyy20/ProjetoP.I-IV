using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se é um NPC
        if (other.CompareTag("NPC"))
        {
            Debug.Log("Vítima " + other.gameObject.name + " está segura!");
            // Para o movimento
            NPCRandomWalk npc = other.GetComponent<NPCRandomWalk>();
            if (npc != null)
            {
                npc.StopMovement(); // Método que vamos garantir que exista
            }
        }
    }
}
