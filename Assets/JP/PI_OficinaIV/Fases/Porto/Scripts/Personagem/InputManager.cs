using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    PlayerControls playerControls;
//    [SerializeField] Vector2 inputMovimento;

//    public float inputHorizontal;
//    public float inputVertical;

//    private void OnEnable()
//    {
//        if(playerControls == null)
//        {
//            playerControls = new PlayerControls();
//            playerControls.Movimentacao.Moviment.performed += i => inputMovimento = i.ReadValue<Vector2>();
//        }

//        playerControls.Enable();
//    }

//    private void OnDisable()
//    {
//        playerControls.Disable();
//    }

//    void InputsMovimento()
//    {
//        inputVertical = inputMovimento.y;
//        inputHorizontal = inputMovimento.x;
//    }

//    public void RodarInputs()
//    {
//        InputsMovimento();
//    }
//}
