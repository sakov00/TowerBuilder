using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace UI.SpecialItems
{
    // Класс для обработки нажатия и удержания кнопки UI
    public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // События, которые можно подписывать
        public UnityEvent OnPressed;
        public UnityEvent OnReleased;

        // Флаг удержания кнопки
        public bool IsHolding { get; private set; } = false;

        // Вызывается, когда кнопка нажата
        public void OnPointerDown(PointerEventData eventData)
        {
            IsHolding = true;
            OnPressed?.Invoke();
        }

        // Вызывается, когда кнопка отпущена
        public void OnPointerUp(PointerEventData eventData)
        {
            IsHolding = false;
            OnReleased?.Invoke();
        }
    }
}