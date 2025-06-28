using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    MoveJogador moveJogador;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        moveJogador = GetComponent<MoveJogador>();
    }

    private void Update()
    {
        inputManager.RodarInputs();
    }

    private void FixedUpdate()
    {
        moveJogador.RodarInputsJogador();
    }
}
