using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationShower : MonoBehaviour
{
    [SerializeField] private GameObject notificationObject;
    [SerializeField] private float showDuration = 2f;
    [SerializeField] private TMP_Text notificationText;

    void OnEnable()
    {
        notificationObject.SetActive(false);
    }

    public void ShowNotification(string message)
    {
        if(notificationObject.activeSelf)
        {
            StopAllCoroutines();
            notificationObject.SetActive(false);
        }
        notificationText.text = message;
        notificationObject.SetActive(true);
        StartCoroutine(DisableNotification());
    }

    IEnumerator DisableNotification()
    {
        yield return new WaitForSeconds(showDuration);

        notificationObject.SetActive(false);
    }
}
