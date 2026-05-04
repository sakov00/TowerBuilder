using UnityEngine;

namespace _Project.Scripts.UI.Rendering
{
    [RequireComponent(typeof(RectTransform))]
    public class SetAnchorsToCorners : MonoBehaviour
    {
        [ContextMenu("Set Anchors To Corners")]
        void SetAnchors()
        {
            var rectTransform = GetComponent<RectTransform>();
            var parentRect = rectTransform.parent as RectTransform;
            if (parentRect == null)
            {
                Debug.LogWarning("RectTransform has no parent. Anchors cannot be set.");
                return;
            }

            Vector2 anchorMin = new Vector2(
                rectTransform.anchorMin.x + rectTransform.offsetMin.x / parentRect.rect.width,
                rectTransform.anchorMin.y + rectTransform.offsetMin.y / parentRect.rect.height
            );

            Vector2 anchorMax = new Vector2(
                rectTransform.anchorMax.x + rectTransform.offsetMax.x / parentRect.rect.width,
                rectTransform.anchorMax.y + rectTransform.offsetMax.y / parentRect.rect.height
            );

            // Undo.RecordObject(rectTransform, "Set Anchors To Corners");
            
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}