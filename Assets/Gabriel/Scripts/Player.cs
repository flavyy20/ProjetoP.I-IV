using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera cam1;
    public float velocidade = 5f;

    private void Update() => Move();

    private void FixedUpdate() => cam1.transform.LookAt(transform);

    private void Move()
    {
        Vector3 movement = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        transform.Translate(Time.deltaTime * velocidade * movement);
    }
}