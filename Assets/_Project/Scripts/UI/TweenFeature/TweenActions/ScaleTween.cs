using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class ScaleTween : TweenAction
    {
        [SerializeField] private Transform _target;

        [Header("Portrait")]
        [SerializeField] private Vector3 _scaleDeltaPortrait = Vector3.one;
        [SerializeField] private Vector3 _minScalePortrait = Vector3.one * 0.5f;
        [SerializeField] private Vector3 _maxScalePortrait = Vector3.one * 2f;

        [Header("Landscape")]
        [SerializeField] private Vector3 _scaleDeltaLandscape = Vector3.one;
        [SerializeField] private Vector3 _minScaleLandscape = Vector3.one * 0.5f;
        [SerializeField] private Vector3 _maxScaleLandscape = Vector3.one * 2f;

        [Header("Tween Settings")]
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay = 0f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        private int _width;
        private int _height;

        private void OnValidate()
        {
            if (_target == null)
                Debug.LogWarning($"{nameof(ScaleTween)}: Target не найден на {gameObject.name}");
        }

        private void Awake()
        {
            _width = Screen.width;
            _height = Screen.height;

            if (_playOnAwake)
                GetTween().Play();
        }

        private void Update()
        {
            if (_checkState && (_width != Screen.width || _height != Screen.height))
            {
                _width = Screen.width;
                _height = Screen.height;
                GetTween().Play();
            }
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(ScaleTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            bool isPortrait = Screen.height >= Screen.width;

            Vector3 delta = isPortrait ? _scaleDeltaPortrait : _scaleDeltaLandscape;
            Vector3 min = isPortrait ? _minScalePortrait : _minScaleLandscape;
            Vector3 max = isPortrait ? _maxScalePortrait : _maxScaleLandscape;

            // Рассчитываем новый масштаб с учётом текущего
            Vector3 newScale = _target.localScale + delta;

            // Ограничиваем по мин/макс
            newScale.x = Mathf.Clamp(newScale.x, min.x, max.x);
            newScale.y = Mathf.Clamp(newScale.y, min.y, max.y);
            newScale.z = Mathf.Clamp(newScale.z, min.z, max.z);

            return _target
                .DOScale(newScale, _duration)
                .SetEase(_ease)
                .SetDelay(_delay);
        }
    }
}
