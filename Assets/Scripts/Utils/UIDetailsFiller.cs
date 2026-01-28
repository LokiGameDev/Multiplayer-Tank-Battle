using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UIDetailsFiller : NetworkBehaviour
{
    [SerializeField] private TMP_Text joinCodeText;

    public void Start()
    {
        if(!IsServer)
        {
            joinCodeText.text = "";
        }
        else
        {
            joinCodeText.gameObject.SetActive(true);
            joinCodeText.text = "Code: " + HostSingleton.Instance?.GameManager.joinCode;    
        }
    }
}
