using System;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class AnchorTween : TweenAction
    {
        [SerializeField] private RectTransform _target;

        [Header("Portrait Anchors & Position")]
        [SerializeField] private Vector2 _anchorMinPortrait = new Vector2(0f, 1f);
        [SerializeField] private Vector2 _anchorMaxPortrait = new Vector2(0f, 1f);
        [SerializeField] private Vector2 _anchoredPosPortrait = Vector2.zero;
        [SerializeField] private Vector3 _rotationPortrait = Vector3.zero;
        [SerializeField] private Vector3 _scalePortrait = Vector3.one;
        [SerializeField] private Vector2 _pivotPortrait = new Vector2(0.5f, 0.5f);

        [Header("Landscape Anchors & Position")]
        [SerializeField] private Vector2 _anchorMinLandscape = new Vector2(1f, 1f);
        [SerializeField] private Vector2 _anchorMaxLandscape = new Vector2(1f, 1f);
        [SerializeField] private Vector2 _anchoredPosLandscape = Vector2.zero;
        [SerializeField] private Vector3 _rotationLandscape = Vector3.zero;
        [SerializeField] private Vector3 _scaleLandscape = Vector3.one;
        [SerializeField] private Vector2 _pivotLandscape = new Vector2(0.5f, 0.5f);

        [Header("Tween Settings")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        private bool _lastIsPortrait;

        private void OnValidate()
        {
            if (_target == null)
                Debug.LogWarning($"{nameof(AnchorTween)}: Target не найден на {gameObject.name}");
        }

        private void Awake()
        {
            _lastIsPortrait = Screen.height > Screen.width;

            if (_playOnAwake)
                GetTween().Play();
        }

        private void Update()
        {
            if (!_checkState)
                return;

            bool isPortrait = Screen.height > Screen.width;

            if (isPortrait != _lastIsPortrait)
            {
                _lastIsPortrait = isPortrait;
                GetTween().Play();
            }
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(AnchorTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            bool isPortrait = Screen.height > Screen.width;

            Vector2 anchorMin = isPortrait ? _anchorMinPortrait : _anchorMinLandscape;
            Vector2 anchorMax = isPortrait ? _anchorMaxPortrait : _anchorMaxLandscape;
            Vector2 anchoredPos = isPortrait ? _anchoredPosPortrait : _anchoredPosLandscape;
            Vector3 rotation = isPortrait ? _rotationPortrait : _rotationLandscape;
            Vector3 scale = isPortrait ? _scalePortrait : _scaleLandscape;
            Vector2 pivot = isPortrait ? _pivotPortrait : _pivotLandscape;

            var seq = DOTween.Sequence();

            // Сбрасываем offset'ы, чтобы anchorMin/Max работали стабильно
            // _target.offsetMin = new Vector2(_target.offsetMin.x, 0f);
            // _target.offsetMax = new Vector2(_target.offsetMax.x, 0f);

            seq.Join(DOTween.To(() => _target.anchorMin, x => _target.anchorMin = x, anchorMin, _duration))
               .Join(DOTween.To(() => _target.anchorMax, x => _target.anchorMax = x, anchorMax, _duration))
               .Join(_target.DOAnchorPos(anchoredPos, _duration))
               .Join(_target.DOLocalRotate(rotation, _duration))
               .Join(_target.DOScale(scale, _duration))
               .Join(_target.DOPivot(pivot, _duration))
               .SetEase(_ease)
               .SetDelay(_delay);

            return seq;
        }
    }
}
