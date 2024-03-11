using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DeviceType
{
    UNKNOWN,
    GAMEPAD,
    KEYBOARD
}
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControl;
    [SerializeField] PlayerInput playerInput;
    private string actionMapKey = "Player";

    private string moveActionKey = "Move";
    private string rotateLeftActionKey = "RotateLeft";
    private string rotateRightActionKey = "RotateRight";

    public InputAction MoveAction { get; private set; }
    public InputAction RotateLeftAction { get; private set; }
    public InputAction RotateRightAction { get; private set; }

    public static PlayerInputController Instance;

    private void Awake()
    {
        Instance = this;

        MoveAction = playerControl.FindActionMap(actionMapKey).FindAction(moveActionKey);
        RotateLeftAction = playerControl.FindActionMap(actionMapKey).FindAction(rotateLeftActionKey);
        RotateRightAction = playerControl.FindActionMap(actionMapKey).FindAction(rotateRightActionKey);
    }

    private void OnEnable()
    {
        MoveAction?.Enable();
        RotateLeftAction?.Enable();
        RotateRightAction?.Enable();
    }

    private void OnDisable()
    {
        MoveAction?.Disable();
        RotateLeftAction?.Disable();
        RotateRightAction?.Disable();
    }

}
