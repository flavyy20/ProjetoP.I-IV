using UnityEngine;
using System.Collections.Generic;

public class FloorFallManager : MonoBehaviour
{
    public float fallDelay = 5f;  // intervalo entre quedas

    private List<FloorFall> floors = new List<FloorFall>();

    private void Start()
    {
        // Busca todos os pisos com o script FloorFall na cena
        FloorFall[] foundFloors = FindObjectsOfType<FloorFall>();
        floors.AddRange(foundFloors);

        // Inicia a repetição do método que escolhe pisos para cair
        InvokeRepeating(nameof(TriggerRandomFloor), fallDelay, fallDelay);
    }

    private void TriggerRandomFloor()
    {
        // Remove os pisos que já estão caindo ou que já caíram
        floors.RemoveAll(f => f == null);  // limpa pisos destruídos

        if (floors.Count == 0)
        {
            // Para a repetição se não tiver mais pisos
            CancelInvoke(nameof(TriggerRandomFloor));
            return;
        }

        // Escolhe um piso aleatório da lista
        int index = Random.Range(0, floors.Count);
        FloorFall chosenFloor = floors[index];

        // Inicia o blink e queda
        chosenFloor.StartBlink();

        // Remove o piso escolhido da lista para não cair de novo
        floors.RemoveAt(index);
    }
}
