using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenActions
{
    public class FadeTween : TweenAction
    {
        [Header("Targets")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _image;

        [Header("Settings")]
        [SerializeField] private bool _fadeIn = true;
        [SerializeField] private bool _useCurrentAsStart = false;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _controlRaycast = true;

        private void OnValidate()
        {
            if (_canvasGroup == null && _image == null)
            {
                Debug.LogWarning($"{nameof(FadeTween)}: не назначен ни один целевой компонент на {gameObject.name}");
            }
        }

        public override Tween GetTween()
        {
            if (_canvasGroup == null && _image == null)
            {
                Debug.LogWarning($"{nameof(FadeTween)}: не назначен ни один целевой компонент на {gameObject.name}");
                return DOTween.Sequence();
            }

            float start, end;
            if (_useCurrentAsStart)
            {
                start = _canvasGroup != null ? _canvasGroup.alpha : _image.color.a;
            }
            else
            {
                start = _fadeIn ? 0f : 1f;
                if (_canvasGroup != null)
                    _canvasGroup.alpha = start;
                else
                    SetImageAlpha(start);
            }

            end = _fadeIn ? 1f : 0f;

            Tween tween;
            if (_canvasGroup != null)
                tween = _canvasGroup.DOFade(end, _duration);
            else
                tween = _image.DOFade(end, _duration);

            tween.SetEase(_ease).SetDelay(_delay);

            if (_controlRaycast)
            {
                tween.OnStart(() =>
                {
                    if (!_fadeIn)
                        SetRaycast(false);
                });

                tween.OnComplete(() =>
                {
                    if (_fadeIn)
                        SetRaycast(true);
                });
            }

            return tween;
        }

        private void SetImageAlpha(float value)
        {
            if (_image == null) return;
            var color = _image.color;
            color.a = value;
            _image.color = color;
        }

        private void SetRaycast(bool state)
        {
            if (_canvasGroup != null)
                _canvasGroup.blocksRaycasts = state;
            else if (_image != null)
                _image.raycastTarget = state;
        }

        public void Play()
        {
            GetTween().Play();
        }
    }
}
