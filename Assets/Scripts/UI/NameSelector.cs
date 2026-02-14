using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button connectButton;
    [SerializeField] private NotificationShower notificationShower;
    [SerializeField] private int minNameLength = 1;
    [SerializeField] private int maxNameLength = 12;
    public const string PlayerNameKey = "PlayerName";

    private void Start()
    {
        if(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
        nameField.text = PlayerPrefs.GetString(PlayerNameKey, string.Empty);
        HandleNameChange();
    }

    public void HandleNameChange()
    {
        if(nameField.text.Length <= minNameLength || nameField.text.Length >= maxNameLength)
        {
            notificationShower.ShowNotification($"Name must be between {minNameLength} and {maxNameLength} characters.");
        }
    }

    public void Connect()
    {
        if(nameField.text.Length <= minNameLength || nameField.text.Length >= maxNameLength)
        {
            notificationShower.ShowNotification($"Name must be between {minNameLength} and {maxNameLength} characters.");
            return;
        }
        PlayerPrefs.SetString(PlayerNameKey, nameField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
