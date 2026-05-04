using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.SpecialItems.Paint
{
    public class FingerPainterController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [Header("Painter Settings")]
        public List<FingerDrawerGPU> targets;
        public List<ImageStampDrawer> stamptargets;
        public List<RectTransform> rectTransformTargets;
        public List<Color> colors;
        public List<Texture2D> patterns;
        public List<Texture2D> stamps;

        [Tooltip("Включает/отключает обработку событий")]
        [SerializeField] public bool _isEnabled = true;
        [SerializeField] public bool _isStamped = false;
        [SerializeField] public UnityEvent filledEvent;
        [SerializeField] public UnityEvent stampedEvent;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => _isEnabled = value;
        }
        
        public bool IsStamped
        {
            get => _isStamped;
            set => _isStamped = value;
        }

        public UnityEvent OnPointerDowned;
        public UnityEvent OnDragging;

        // Храним предыдущую позицию для интерполяции
        private Vector2? previousPointerPos = null;
        private bool isDrawing = false; // флаг, чтобы начать рисование только внутри RectTransform

        public void OnPointerDown(PointerEventData e)
        {
            if (!IsEnabled) return;

            isDrawing = true;
            for (int i = 0; i < rectTransformTargets.Count; i++)
            {
                var rect = rectTransformTargets[i];
                if (rect != null && RectTransformUtility.RectangleContainsScreenPoint(rect, e.position, e.pressEventCamera))
                {
                    isDrawing = true;
                    break;
                }
            }

            if (isDrawing && IsStamped == false)
            {
                Paint(e, true);
                OnPointerDowned?.Invoke();
            }
        }

        public void OnDrag(PointerEventData e)
        {
            if (!IsEnabled || !isDrawing || IsStamped == true) return;
            Paint(e, false);
            OnDragging?.Invoke();
        }

        private void Paint(PointerEventData e, bool isPointerDown)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                var t = targets[i];
                if (!t.gameObject.activeInHierarchy) continue;

                var rect = rectTransformTargets[i];
                if (rect == null) continue;

                Vector2 currentPos = e.position;

                if (isPointerDown || previousPointerPos == null)
                {
                    t.HandlePointer(currentPos);
                }
                else
                {
                    Vector2 prevPos = previousPointerPos.Value;
                    float distance = Vector2.Distance(prevPos, currentPos);
                    int steps = Mathf.CeilToInt(distance / 5f);

                    for (int s = 1; s <= steps; s++)
                    {
                        Vector2 interpolated = Vector2.Lerp(prevPos, currentPos, s / (float)steps);

                        // Ограничение можно оставить, если нужно рисовать только внутри RectTransform
                        if (RectTransformUtility.RectangleContainsScreenPoint(rect, interpolated, e.pressEventCamera))
                        {
                            t.HandlePointer(interpolated);
                        }
                        else
                        {
                            // Если хотите, можно продолжать рисовать даже за пределами, просто уберите проверку выше
                            // t.HandlePointer(interpolated);
                        }
                    }
                }
            }

            previousPointerPos = e.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            previousPointerPos = null;
            if(IsStamped == false)
                CheckFill();
        }

        public void CheckFill()
        {
            foreach (var target in targets)
            {
                if (!target.gameObject.activeInHierarchy) continue;

                var percent = target.GetFillPercent();
                if(percent > 0.75) filledEvent?.Invoke();
            }
        }

        public void SetColor(int index)
        {
            targets.ForEach(x=> x.brushColor = colors[index]);
        }
        
        public void SetPattern(int index)
        {
            targets.ForEach(x=> x.brushPattern = patterns[index]);
        }
        
        public void SetStamp(int index)
        {
            stamptargets.ForEach(x=> x.stampTexture = stamps[index]);
        }
        
        public void SetPatternAlpha(float value)
        {
            targets.ForEach(x=> x.brushAlphaPattern = value);
        }
        
        public void SetEnabled(bool enabled)
        {
            targets.ForEach(x=> x.SetEnabled(enabled));
        }
        
        public void SetUsePattern(bool enabled)
        {
            targets.ForEach(x=> x.SetUsePattern(enabled));
        }

        private void OnDisable()
        {
            previousPointerPos = null;
            isDrawing = false;
        }
    }
}
