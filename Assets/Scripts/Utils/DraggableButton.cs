using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableButton : MonoBehaviour, IDragHandler
{
    public RectTransform rectTransform;
    public Canvas Canvas;

    private Vector2 boundariesMin = new Vector2(-820, -310);
    private Vector2 boundariesMax = new Vector2(820, 310);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = rectTransform.anchoredPosition + eventData.delta / Canvas.scaleFactor;
        newPosition.x = Mathf.Clamp(newPosition.x, boundariesMin.x, boundariesMax.x);
        newPosition.y = Mathf.Clamp(newPosition.y, boundariesMin.y, boundariesMax.y);
        rectTransform.anchoredPosition = newPosition;
        // rectTransform.anchoredPosition += eventData.delta/Canvas.scaleFactor;
    }
}
