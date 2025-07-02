using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EstadoCidade
{
    public string cityName;
    public float progress = 0f;
    public bool isEventActive = false; 

    public void StartEvent()
    {
        isEventActive = true;
        progress = 0f;
        Debug.Log($"{cityName}: Evento iniciado!");
    }

    public void ResolveEvent()
    {
        isEventActive = false;
        progress = 0f;
        Debug.Log($"{cityName}: Evento resolvido!");
    }
}
