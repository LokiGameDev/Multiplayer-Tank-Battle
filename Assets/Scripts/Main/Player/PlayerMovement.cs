using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D bodyRigidbody;
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float turningRate = 270f;
    [SerializeField] private Transform visualChild;

    private Vector2 previousMovementInput;
    private Vector2 mobileMovementInput;
    private bool isMobile;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) return;
        inputReader.PlayerMovementEvent += HandleMovement;
        inputReader.MobilePlayerMovement += HandleMobileMovement;
    }

    public override void OnNetworkDespawn()
    {
        if(!IsOwner) return;
        inputReader.PlayerMovementEvent -= HandleMovement;
        inputReader.MobilePlayerMovement -= HandleMobileMovement;
    }

    private void HandleMovement(Vector2 movementInput)
    {
        isMobile = false;
        previousMovementInput = movementInput;
        Debug.Log(previousMovementInput);
    }

    private void HandleMobileMovement(Vector2 mobileInput)
    {
        isMobile = true;
        mobileMovementInput = mobileInput;
        Debug.Log(mobileInput);
    }

    private void Update()
    {
        if(!IsOwner) return;

        float zRotation = previousMovementInput.x * -turningRate * Time.deltaTime;
        bodyTransform.Rotate(0,0,zRotation);
    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;
        if(!isMobile) bodyRigidbody.linearVelocity = (Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;
        else
        {
            bodyRigidbody.linearVelocity = mobileMovementInput * movementSpeed;
            Vector2 moveDir = bodyRigidbody.linearVelocity;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

                // Adjust offset depending on sprite facing
                Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90f);

                visualChild.rotation = Quaternion.Lerp(
                    visualChild.rotation,
                    targetRot,
                    turningRate * Time.deltaTime
                );
            }
        }
    }
}
