using Unity.Netcode;
using UnityEngine;

public class PlayerAiming : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretTransform;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Vector2 mobileAimInput;

    private float rotationSpeed = 270f;

    private void LateUpdate()
    {
        if(!IsOwner) return;
        if(!inputReader.isMobile)
        {
            Vector2 aimScreenPosition = inputReader.aimPosition;
            Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);
            turretTransform.up = aimWorldPosition - (Vector2)transform.position;
        }
        else
        {
            mobileAimInput = inputReader.mobileAimPosition;
            if (mobileAimInput.sqrMagnitude > 0.01f)
            {
                float angle = Mathf.Atan2(mobileAimInput.y, mobileAimInput.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, 0, angle - 90f);

                turretTransform.rotation = Quaternion.Lerp(
                    turretTransform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
