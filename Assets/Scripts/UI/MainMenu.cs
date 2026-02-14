using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeField;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private GameObject mobileControlsChangePanel;
    private const string PlayerNameKey = "PlayerName";
    public void OnEnable()
    {
        mobileControlsChangePanel.SetActive(Application.isMobilePlatform);
        playerNameText.text = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
    }
    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }
}
