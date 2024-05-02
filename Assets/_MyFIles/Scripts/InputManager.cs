using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInput playerInput;

    public Vector2 moveInput { get; private set; }

    bool bCanMove = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        playerInput = new PlayerInput();
        playerInput.Enable();

        playerInput.Main.ExitGame.performed += ExitGame;
        playerInput.Main.StartButton.performed += StartGame;
    }

    private void ExitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    private void Update()
    {
        if (!GameManager.Instance.HasGameStarted()) return;

        if (GetInputActive() == false)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = MoveInput();
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.HasGameStarted()) return;

        GameManager.Instance.StartGame();
    }

    public bool GetInputActive()
    {
        return bCanMove;
    }

    public void SetInputActive(bool state)
    {
        bCanMove = state;
    }

    private Vector2 MoveInput()
    {
        Vector2 moveInput = playerInput.Main.Move.ReadValue<Vector2>();

        return moveInput; 
    }


}
