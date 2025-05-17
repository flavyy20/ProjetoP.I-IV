using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GroundSink : MonoBehaviour
{
    public float sinkAmount = 20f;
    public float sinkSpeed = 3f;
    public bool isSinking = false;

    private Collider groundCollider;
    private Bounds groundBounds;
    private NavMeshModifier navModifier;

    public bool IsUnsafe => isSinking;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Ground");
        groundCollider = GetComponent<Collider>();
        groundBounds = groundCollider.bounds;

        navModifier = GetComponent<NavMeshModifier>();
        if (navModifier == null)
        {
            navModifier = gameObject.AddComponent<NavMeshModifier>();
            navModifier.overrideArea = true;
            navModifier.area = 2; // Jump
            navModifier.ignoreFromBuild = false;
        }
    }

    public Bounds GetBounds() => groundBounds;

    public IEnumerator Sink()
    {
        if (isSinking) yield break;

        isSinking = true;
        gameObject.tag = "Unsafe";

        // Ativa a área de "Jump"
        navModifier.enabled = true;

        Vector3 startPos = transform.position;
        float targetY = startPos.y - sinkAmount;

        while (transform.position.y > targetY)
        {
            transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
