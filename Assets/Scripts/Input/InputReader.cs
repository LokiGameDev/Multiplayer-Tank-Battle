using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> PrimaryFireEvent;
    public event Action MobileFireEvent;
    public event Action<Vector2> PlayerMovementEvent;
    public Vector2 aimPosition { get; private set; }
    public Vector2 mobileAimPosition { get; private set; }
    public bool isMobile {get;private set;}
    private bool wasAiming;

    public event Action<Vector2> MobilePlayerMovement;
    private Controls controls;
    void OnEnable()
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }
    void OnDisable()
    {
        controls.Player.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        PlayerMovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
            PrimaryFireEvent?.Invoke(true);
        else if(context.canceled)
            PrimaryFireEvent?.Invoke(false);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimPosition = context.ReadValue<Vector2>();
    }

    public void OnMobileMove(InputAction.CallbackContext context)
    {
        isMobile = true;
        MobilePlayerMovement?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMobileShoot(InputAction.CallbackContext context)
    {
        isMobile = true;
        if (context.performed)
        {
            mobileAimPosition = context.ReadValue<Vector2>();
            wasAiming = true;
        }

        if (context.canceled)
        {
            mobileAimPosition = Vector2.zero;

            if (wasAiming)
                MobileFireEvent?.Invoke();

            wasAiming = false;
        }
    }
}