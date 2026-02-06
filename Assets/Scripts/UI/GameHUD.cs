using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameHUD : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private GameObject instructionsPanel;

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
        StartCoroutine(TurnOffInstructionsAfterDelay());
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, new Vector2(0,0), CursorMode.Auto);
    }

    IEnumerator TurnOffInstructionsAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<FadeAway>().StartFade(2f);
        yield return new WaitForSeconds(2f);
        instructionsPanel.SetActive(false);
    }
}
