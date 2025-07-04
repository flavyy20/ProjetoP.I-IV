using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool ferido = false;
    public bool resgatado = false;
    public GameObject efeitoResgate;
    private Helicoptero helicoptero;
   

    void Start()
    {
        // Encontra o helicóptero na cena
        helicoptero = FindObjectOfType<Helicoptero>();
    
    }

    void OnMouseDown()
    {
        if (!resgatado && ferido && helicoptero != null)
        {
            helicoptero.IniciarResgate(transform);
        }
    }

    public void Resgatar()
    {
        if (!resgatado)
        {
            Debug.Log("Resgatado");
            resgatado = true;
            
            if (GameM.instance != null)
            {
                GameM.instance.AtualizarNPCsResgatados(1); // Notifica o GameManager corretamente
            }
            if (efeitoResgate != null)
            {
                Instantiate(efeitoResgate, transform.position, Quaternion.identity);
            }

            Destroy(gameObject, 1f); // Destroi após 1.5 segundos
        }
    }
}
