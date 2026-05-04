using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace UI.SpecialItems
{
    public class UICharacterZoom : MonoBehaviour
    {
        public RectTransform characterRect;
        public RectTransform canvasRect;
        public RectTransform panelRect;

        [Header("Zoom settings")]
        public Vector2 headFocus = new Vector2(0.5f, 0.8f); // нормализованные позиции
        public Vector2 bodyFocus = new Vector2(0.5f, 0.5f);
        public Vector2 legsFocus = new Vector2(0.5f, 0.2f);

        public float zoomScale = 1.5f;
        public float animationDuration = 0.5f;

        private Tween currentTween;
        private FocusedPartBody currentState;
        private int _lastWidth; 
        private int _lastHeight;

        private IEnumerator Start()
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            yield return null;
            ResetFocus(false);
        }

        private void LateUpdate()
        {
            if (_lastWidth != Screen.width || _lastHeight != Screen.height)
            {
                _lastWidth = Screen.width;
                _lastHeight = Screen.height;
                
                StartCoroutine(ApplyFocusNextFrame());
            }
        }
        
        private IEnumerator ApplyFocusNextFrame()
        {
            yield return null;

            switch (currentState)
            {
                case FocusedPartBody.Head:
                    SetFocus(headFocus, withTime: false);
                    break;
                case FocusedPartBody.Body:
                    SetFocus(bodyFocus, withTime: false);
                    break;
                case FocusedPartBody.Legs:
                    SetFocus(legsFocus, withTime: false);
                    break;
                case FocusedPartBody.Full:
                    ResetFocus(false);
                    break;
            }
        }

        public void FocusHead()
        {
            currentState = FocusedPartBody.Head;
            SetFocus(headFocus);
        }

        public void FocusBody()
        {
            currentState = FocusedPartBody.Body;
            SetFocus(bodyFocus);
        }

        public void FocusLegs()
        {
            currentState = FocusedPartBody.Legs;
            SetFocus(legsFocus);
        }

        public void ResetFocus(bool withTime = true)
        {
            currentState = FocusedPartBody.Full;
            var normalizedPivot = new Vector2(0.5f, 0.5f);
            var scale = 1f;
            
            if (currentTween != null && currentTween.IsActive()) currentTween.Kill();

            float canvasHeight = canvasRect.rect.height;
            float canvasWidth = canvasRect.rect.width;

            float panelHeight = panelRect.rect.height;
            float panelWidth = panelRect.rect.width;

            Vector2 targetPos = Vector2.zero;

            if (Screen.width > Screen.height)
            {
                float xOffset = panelWidth / 2f;
                targetPos.x = (normalizedPivot.x - 0.5f) * canvasWidth - xOffset;
                targetPos.y = 0f;
            }
            else
            {
                // Вертикальная панель (сверху/снизу)
                float yOffset = panelHeight / 2f;
                targetPos.x = (normalizedPivot.x - 0.5f) * canvasWidth;
                targetPos.y = 0f;
            }

            Vector3 targetScale = scale < 0 ? Vector3.one * zoomScale : Vector3.one * scale;

            if (withTime)
            {
                currentTween = DOTween.Sequence()
                    .Join(characterRect.DOAnchorPos(targetPos, animationDuration).SetEase(Ease.OutCubic))
                    .Join(characterRect.DOScale(targetScale, animationDuration).SetEase(Ease.OutCubic));
            }
            else
            {
                currentTween = DOTween.Sequence()
                    .Join(characterRect.DOAnchorPos(targetPos, 0).SetEase(Ease.OutCubic))
                    .Join(characterRect.DOScale(targetScale, 0).SetEase(Ease.OutCubic));
            }
        }

        private void SetFocus(Vector2 normalizedPivot, float scale = -1f, bool withTime = true)
        {
            if (currentTween != null && currentTween.IsActive()) currentTween.Kill();

            float canvasHeight = canvasRect.rect.height;
            float canvasWidth = canvasRect.rect.width;

            float panelHeight = panelRect.rect.height;
            float panelWidth = panelRect.rect.width;

            Vector2 targetPos = Vector2.zero;

            if (Screen.width > Screen.height)
            {
                float xOffset = panelWidth / 2f;
                targetPos.x = (normalizedPivot.x - 0.5f) * canvasWidth - xOffset;
                targetPos.y = (0.5f - normalizedPivot.y) * canvasHeight;
            }
            else
            {
                // Вертикальная панель (сверху/снизу)
                float yOffset = panelHeight / 2f;
                targetPos.x = (normalizedPivot.x - 0.5f) * canvasWidth;
                targetPos.y = (0.5f - normalizedPivot.y) * canvasHeight + yOffset;
            }

            Vector3 targetScale = scale < 0 ? Vector3.one * zoomScale : Vector3.one * scale;

            
            if (withTime)
            {
                currentTween = DOTween.Sequence()
                    .Join(characterRect.DOAnchorPos(targetPos, animationDuration).SetEase(Ease.OutCubic))
                    .Join(characterRect.DOScale(targetScale, animationDuration).SetEase(Ease.OutCubic));
            }
            else
            {
                currentTween = DOTween.Sequence()
                    .Join(characterRect.DOAnchorPos(targetPos, 0).SetEase(Ease.OutCubic))
                    .Join(characterRect.DOScale(targetScale, 0).SetEase(Ease.OutCubic));
            }
        }
    }

    public enum FocusedPartBody
    {
        Full,
        Head,
        Body,
        Legs,
    }
}
