using Unity.Netcode;
using UnityEngine;

public class GameHUD : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Texture2D cursorTexture;

    public void LeaveGame()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            HostSingleton.Instance.GameManager.Shutdown();
        }

        ClientSingleton.Instance.GameManager.Disconnect();
    }

    private void OnEnable()
    {
        Cursor.SetCursor(cursorTexture, new Vector2(0,0), CursorMode.Auto);
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, new Vector2(0,0), CursorMode.Auto);
    }
}
