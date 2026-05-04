using UnityEngine;

public static class UIUtils
{
    public static Vector2 GetAnchoredPositionRelativeToCanvas(RectTransform slotRect, RectTransform targetRect, Canvas canvas)
    {
        Vector2 slotLocalInCanvas = canvas.transform.InverseTransformPoint(slotRect.position);
        Vector2 anchoredPos = (Vector2)targetRect.parent.InverseTransformPoint(canvas.transform.TransformPoint(slotLocalInCanvas));
        return anchoredPos;
    }
}