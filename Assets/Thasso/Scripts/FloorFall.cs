using UnityEngine;
using UnityEngine.AI;

public class FloorFall : MonoBehaviour
{
    [Header("Tempo de queda personalizado")]
    public float fallTime = 5f; // Agora você define o tempo direto no Inspector

    [Header("Parâmetros da queda")]
    public int blinkCount = 5;
    public float blinkInterval = 0.3f;
    public float fallDistance = 0.6f;

    private Renderer rend;
    private Color originalColor;
    private Vector3 initialPosition;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        initialPosition = transform.position;

        // Agende a queda automática com base no tempo definido no Inspector
        Invoke(nameof(StartBlink), fallTime);
    }

    public void StartBlink()
    {
        StartCoroutine(BlinkThenFall());
    }

    private System.Collections.IEnumerator BlinkThenFall()
    {
        float step = fallDistance / blinkCount;

        for (int i = 0; i < blinkCount; i++)
        {
            if (rend != null)
                rend.material.color = Color.red;

            yield return new WaitForSeconds(blinkInterval);

            if (rend != null)
                rend.material.color = originalColor;

            yield return new WaitForSeconds(blinkInterval);

            transform.position -= new Vector3(0f, step, 0f);
        }

        Fall();
    }

    private void Fall()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");

        if (rend != null)
            rend.material.color = Color.red;

        StopNPCsOnTop();
    }

    private void StopNPCsOnTop()
    {
        Collider floorCollider = GetComponent<Collider>();
        if (floorCollider == null)
            return;

        Vector3 center = floorCollider.bounds.center + Vector3.up * 0.1f;
        Vector3 halfExtents = floorCollider.bounds.extents + new Vector3(0f, 0.5f, 0f);

        Collider[] hits = Physics.OverlapBox(center, halfExtents);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("NPC"))
            {
                NPCRandomWalk npc = hit.GetComponent<NPCRandomWalk>();
                if (npc != null)
                    npc.CaughtByLava();
            }
        }
    }
}
