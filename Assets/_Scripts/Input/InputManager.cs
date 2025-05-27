using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    InputActions inputActions;
    public PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Movement.performed += playerController.OnMoveStarted;
        inputActions.Player.Movement.canceled += playerController.OnMoveStopped;
        inputActions.Player.Fire.performed += playerController.OnFire;
    }

    private void OnDisable()
    {        
        inputActions.Player.Movement.performed -= playerController.OnMoveStarted;
        inputActions.Player.Movement.canceled -= playerController.OnMoveStopped;
        inputActions.Player.Fire.performed -= playerController.OnFire;
        inputActions.Disable();
    }
}
