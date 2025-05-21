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

        // Inicia a repeti��o do m�todo que escolhe pisos para cair
        InvokeRepeating(nameof(TriggerRandomFloor), fallDelay, fallDelay);
    }

    private void TriggerRandomFloor()
    {
        // Remove os pisos que j� est�o caindo ou que j� ca�ram
        floors.RemoveAll(f => f == null);  // limpa pisos destru�dos

        if (floors.Count == 0)
        {
            // Para a repeti��o se n�o tiver mais pisos
            CancelInvoke(nameof(TriggerRandomFloor));
            return;
        }

        // Escolhe um piso aleat�rio da lista
        int index = Random.Range(0, floors.Count);
        FloorFall chosenFloor = floors[index];

        // Inicia o blink e queda
        chosenFloor.StartBlink();

        // Remove o piso escolhido da lista para n�o cair de novo
        floors.RemoveAt(index);
    }
}
