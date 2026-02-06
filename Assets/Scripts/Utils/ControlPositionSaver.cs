using UnityEngine;

public class ControlPositionSaver : MonoBehaviour
{
    public RectTransform moveRectTransform;
    public RectTransform attackRectTransform;

    public void SavePositions()
    {
        PlayerPrefs.SetFloat("MoveButtonPosX", moveRectTransform.anchoredPosition.x);
        PlayerPrefs.SetFloat("MoveButtonPosY", moveRectTransform.anchoredPosition.y);
        PlayerPrefs.SetFloat("AttackButtonPosX", attackRectTransform.anchoredPosition.x);
        PlayerPrefs.SetFloat("AttackButtonPosY", attackRectTransform.anchoredPosition.y);
        PlayerPrefs.Save();
    }

    public void Start()
    {
        LoadControlPositions();
    }

    private void LoadControlPositions()
    {
        if (PlayerPrefs.HasKey("MoveButtonPosX") && PlayerPrefs.HasKey("MoveButtonPosY"))
        {
            float moveX = PlayerPrefs.GetFloat("MoveButtonPosX");
            float moveY = PlayerPrefs.GetFloat("MoveButtonPosY");
            moveRectTransform.anchoredPosition = new Vector2(moveX, moveY);
        }
        else
        {
            // Set default position if no saved position exists
            moveRectTransform.anchoredPosition = new Vector2(-600, -200);
            PlayerPrefs.SetFloat("MoveButtonPosX", moveRectTransform.anchoredPosition.x);
            PlayerPrefs.SetFloat("MoveButtonPosY", moveRectTransform.anchoredPosition.y);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("AttackButtonPosX") && PlayerPrefs.HasKey("AttackButtonPosY"))
        {
            float attackX = PlayerPrefs.GetFloat("AttackButtonPosX");
            float attackY = PlayerPrefs.GetFloat("AttackButtonPosY");
            attackRectTransform.anchoredPosition = new Vector2(attackX, attackY);
        }
        else
        {
            // Set default position if no saved position exists
            attackRectTransform.anchoredPosition = new Vector2(600, -200);
            PlayerPrefs.SetFloat("AttackButtonPosX", attackRectTransform.anchoredPosition.x);
            PlayerPrefs.SetFloat("AttackButtonPosY", attackRectTransform.anchoredPosition.y);
            PlayerPrefs.Save();
        }
    }

    public void ResetPostionsToDefault()
    {
        moveRectTransform.anchoredPosition = new Vector2(-600, -200);
        attackRectTransform.anchoredPosition = new Vector2(600, -200);
        SavePositions();
    }
}
