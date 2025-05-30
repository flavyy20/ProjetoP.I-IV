using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool ferido = false;
    public bool resgatado = false;
    public GameObject efeitoResgate;

    public void Resgatar()
    {
        if (!resgatado)
        {
            resgatado = true;

            if (efeitoResgate != null)
            {
                Instantiate(efeitoResgate, transform.position, Quaternion.identity);
            }

            Destroy(gameObject, 1.5f); // Destroi após 1.5 segundos
        }
    }
}
