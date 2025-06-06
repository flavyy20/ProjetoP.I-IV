using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FloorFall : MonoBehaviour
{
    public float fallTime = 5f;
    public int blinkCount = 5;
    public float blinkInterval = 0.3f;
    public float fallDistance = 0.6f;

    private List<Renderer> renderers = new List<Renderer>();
    private List<Color> originalColors = new List<Color>();

    void Start()
    {
        EnsureCollidersAndLayers();

        renderers.Clear();
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        originalColors.Clear();
        foreach (var rend in renderers)
        {
            originalColors.Add(rend.material.color);
        }

        Invoke(nameof(StartBlink), fallTime);
    }

    void EnsureCollidersAndLayers()
    {
        int layer = gameObject.layer;

        if (GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();

        foreach (Transform child in transform)
        {
            child.gameObject.layer = layer;

            bool hasRenderer = child.GetComponent<MeshRenderer>() != null;
            bool hasCollider = child.GetComponent<Collider>() != null;

            if (hasRenderer && !hasCollider)
            {
                if (child.GetComponent<MeshFilter>() != null)
                    child.gameObject.AddComponent<MeshCollider>();
                else
                    child.gameObject.AddComponent<BoxCollider>();
            }
        }
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
            SetRenderersColor(Color.red);
            yield return new WaitForSeconds(blinkInterval);

            SetRenderersToOriginalColors();
            yield return new WaitForSeconds(blinkInterval);

            transform.position -= new Vector3(0f, step, 0f);
        }

        Fall();
    }

    void SetRenderersColor(Color color)
    {
        foreach (var rend in renderers)
            if (rend != null) rend.material.color = color;
    }

    void SetRenderersToOriginalColors()
    {
        for (int i = 0; i < renderers.Count; i++)
            if (renderers[i] != null) renderers[i].material.color = originalColors[i];
    }

    private void Fall()
    {
        int defaultLayer = LayerMask.NameToLayer("Default");
        gameObject.layer = defaultLayer;
        foreach (Transform child in transform)
            child.gameObject.layer = defaultLayer;

        SetRenderersColor(Color.red);

        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false; // remove o chão fisicamente
        }

        StartCoroutine(MoveDownRoutine());
    }

    private IEnumerator MoveDownRoutine()
    {
        float elapsed = 0f;
        float duration = 3f; // tempo que vai demorar para o chão descer
        float distance = 120f; // quanto vai descer no total

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - new Vector3(0f, distance, 0f);

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
}