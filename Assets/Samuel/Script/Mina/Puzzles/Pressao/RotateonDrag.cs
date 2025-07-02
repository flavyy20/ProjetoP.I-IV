using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateonDrag : MonoBehaviour, IDragHandler
{
    public float rotationSpeed = 5f;
    private float currentRotation = 0f;

    public void OnDrag(PointerEventData eventData)
    {
        float deltaX = eventData.delta.x;
        currentRotation += deltaX * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, -currentRotation);
    }
}
