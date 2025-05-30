using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ChamarBarco : MonoBehaviour
{
    private IA_Barco barco;
    Vector3 posicaoInicial;
    private CharacterController _controller;

    void Start()
    {
        barco = FindObjectOfType<IA_Barco>();
        posicaoInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AcionarBarco();
        }
    }

    void AcionarBarco()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Chamou");
            barco?.MoverBarco(hit.point);
        }
    }

    //VAI SER DELETADO DEPOIS. SERVE PARA QUE O JOGADOR VOLTE ATÉ A POSICAO INICIAL ASSIM QUE CAIR NA AGUA
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Agua"))
        {
            Debug.Log("Atravessou");

            _controller.enabled = false;
            transform.position = posicaoInicial;
            _controller.enabled = true;
        }
    }
}
