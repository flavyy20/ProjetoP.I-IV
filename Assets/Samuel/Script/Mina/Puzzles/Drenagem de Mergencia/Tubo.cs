using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Tubo : MonoBehaviour
{


    public string idTubo; // Identificador único (ex: "Tubo1", "Tubo2")
    private Vector3 posicaoOriginal;
    private bool arrastando;
    private float offsetZ;

    void Start()
    {
        posicaoOriginal = transform.position;
        offsetZ = transform.position.z;
    }

    void OnMouseDown()
    {
        arrastando = true;
        offsetZ = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        if (arrastando)
        {
            // Movimento X/Y
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, offsetZ);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        }
    }

    void Update()
    {
        if (arrastando)
        {
            // Movimento no eixo Z com scroll
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            transform.Translate(0, 0, scroll * 1f, Space.World);
        }
    }

    void OnMouseUp()
    {
        arrastando = false;
        VerificarEncaixe();
    }

    void VerificarEncaixe()
    {
        
        foreach (Conector conector in PuzzleManager.instance.conectores)
        {
            if (conector.idTubo == this.idTubo)
            {
                float distancia = Vector3.Distance(transform.position, conector.transform.position);
                if (distancia < PuzzleManager.instance.distanciaEncaixe)
                {
                    transform.position = conector.transform.position;
                    transform.rotation = conector.transform.rotation;
                    GetComponent<Renderer>().material.color = Color.green;
                    conector.estaOcupado = true;

                    PuzzleManager.instance.VerificarCompleto(); 
                    return;
                }
            }
        }

        // Se não encaixou, volta para a posição original
        transform.position = posicaoOriginal;
        GetComponent<Renderer>().material.color = Color.red;
    }
}
