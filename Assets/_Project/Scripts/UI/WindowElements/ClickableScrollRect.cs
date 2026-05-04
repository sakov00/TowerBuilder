using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.WindowElements
{
    public class ClickableScrollRect : ScrollRect, IPointerClickHandler
    {
        private bool _isDragging = false;

        public override void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            base.OnEndDrag(eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isDragging && eventData.pointerPress != null)
            {
                ExecuteEvents.ExecuteHierarchy(
                    eventData.pointerPress,
                    eventData,
                    ExecuteEvents.pointerClickHandler
                );
            }
        }
    }
}