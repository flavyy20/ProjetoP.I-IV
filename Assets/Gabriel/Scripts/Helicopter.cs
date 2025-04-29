using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Helicopter : MonoBehaviour
{
    // Raycast configs
    [SerializeField] Camera cam1, cam2, cam3;
    [SerializeField] Color circleColor = Color.yellow;
    [SerializeField] Material material;

    [SerializeField, Range(8, 256)] int segments = 64;
    [SerializeField] float raio = 2f, lineWidth = 0.1f;
    
    LineRenderer lineRenderer;
    bool toggle, switchCamera;

    // Heli configs
    [SerializeField] float speed = 2f, rotationSpeed = 5f;
    [SerializeField] Transform floor, bote;
    [SerializeField] Color ropeColor = Color.black;

    Transform target;

    private float currentLength = 0f;
    public float ropeLength = 10f, deploySpeed = 2f;

    bool moving;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = segments;
    }

    private void Update()
    {
        ButtonsFunc();
        SwitchCamera();
        Raycast();

        if (moving)
        {
            cam1.enabled = false;
            cam2.enabled = false;
            cam3.enabled = true;
            cam3.transform.LookAt(target);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (toggle) lineRenderer.enabled = focus;
    }

    void SetDestination(Vector3 position)
    {
        if (moving) return;
        StartCoroutine(MoveHelicopter(position));
    }

    IEnumerator MoveHelicopter(Vector3 position)
    {
        toggle = false;
        moving = true;
        target = transform;
        material.color = ropeColor;

        // Rotaciona o heli para a posição
        Vector3 direction = (position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // Move o heli até a posição
        while (DistanceWithoutHeight(transform.position, position) > 1f)
        {
            transform.position = LerpWithoutHeight(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }

        // Corrige a rotação do heli e deixa ele paralelo ao chao
        Vector3 euler = transform.rotation.eulerAngles;
        Quaternion flatRotation = Quaternion.Euler(0f, euler.y, 0f);
        while (Quaternion.Angle(transform.rotation, flatRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, flatRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        cam3.fieldOfView /= 2;
        target = bote;
        bote.gameObject.SetActive(true);

        while (currentLength < ropeLength)
        {
            currentLength += deploySpeed * Time.deltaTime;
            currentLength = Mathf.Min(currentLength, ropeLength);
            bote.position = transform.position + Vector3.down * currentLength;
            UpdateRope();
            yield return null;
        }

        moving = false;
        yield return null;
        cam1.enabled = true;
        cam3.enabled = false;
        cam3.fieldOfView *= 2;
    }

    void Raycast()
    {
        lineRenderer.enabled = toggle;
        if (moving) material.color = circleColor;

        if (toggle && switchCamera)
        {
            Ray ray = cam2.ScreenPointToRay(Input.mousePosition);

            Plane plane = new(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);
                DrawCircle(worldPos);
                if (Input.GetMouseButtonDown(0))
                    SetDestination(worldPos);
            }
        }
    }

    private void DrawCircle(Vector3 center)
    {
        float delta = 2f * Mathf.PI / segments;
        Vector3[] points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            float theta = i * delta;
            points[i] = new Vector3(
                center.x + Mathf.Cos(theta) * raio,
                center.y,
                center.z + Mathf.Sin(theta) * raio
            );
        }
        lineRenderer.SetPositions(points);
    }

    void UpdateRope()
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = bote.position;
        Vector3[] ropePositions = new Vector3[segments];

        print($"startPoint: {startPoint}, endPoint: {endPoint}");

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / (segments - 1);
            ropePositions[i] = Vector3.Lerp(startPoint, endPoint, t);
        }

        lineRenderer.SetPositions(ropePositions);
    }

    void ButtonsFunc()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            switchCamera = true;
            toggle = switchCamera;
        }
    }

    void SwitchCamera()
    {
        cam1.enabled = !switchCamera;
        cam2.enabled = switchCamera;
    }

    float DistanceWithoutHeight(Vector3 a, Vector3 b)
    {
        Vector3 _a = new()
        {
            x = a.x,
            z = a.z
        },
        _b = new()
        {
            x = b.x,
            z = b.z
        };

        return Vector3.Distance(_a, _b);
    }

    Vector3 LerpWithoutHeight(Vector3 a, Vector3 b, float t)
    {
        Vector3 _b = new()
        {
            x = b.x,
            y = a.y,
            z = b.z
        };

        return Vector3.Lerp(a, _b, t);
    }
}