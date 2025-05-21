using UnityEngine;
using UnityEngine.AI;
//using Unity.AI.Navigation;

public class FloorFall : MonoBehaviour
{
    public float fallDelay = 5f;
    public int blinkCount = 5;
    public float blinkInterval = 0.3f;
    public float fallDistance = 0.6f;  // Distância total que o piso desce durante o blink

    private NavMeshSurface navMeshSurface;
    private Renderer rend;
    private Color originalColor;
    private Vector3 initialPosition;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        navMeshSurface = UnityEngine.Object.FindAnyObjectByType<NavMeshSurface>();

        initialPosition = transform.position;

        //Invoke(nameof(StartBlink), fallDelay - (blinkCount * blinkInterval * 2f));
    }

    public void StartBlink()
    {
        StartCoroutine(BlinkThenFall());
    }

    System.Collections.IEnumerator BlinkThenFall()
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

            // Faz o chão afundar um pouco a cada ciclo
            transform.position -= new Vector3(0f, step, 0f);
        }

        Fall();
    }

    void Fall()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");

        if (rend != null)
            rend.material.color = Color.red;

        StopNPCsOnTop();

        /*if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();*/
    }

    void StopNPCsOnTop()
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