using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class MoveToObjectTween : TweenAction
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _aimObjectPortrait;   // объект для вертикальной
        [SerializeField] private RectTransform _aimObjectLandscape;  // объект для горизонтальной
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        private int _width;
        private int _height;
        private void OnValidate()
        {
            if (_target == null || (_aimObjectPortrait == null && _aimObjectLandscape == null))
            {
                Debug.LogWarning($"{nameof(MoveToObjectTween)}: Target или AimObjects не указаны на {gameObject.name}");
            }
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

        private RectTransform GetCurrentAimObject()
        {
            // Проверяем ориентацию экрана
            bool isPortrait = Screen.height >= Screen.width;

            if (isPortrait)
                return _aimObjectPortrait != null ? _aimObjectPortrait : _aimObjectLandscape;
            else
                return _aimObjectLandscape != null ? _aimObjectLandscape : _aimObjectPortrait;
        }
        
        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(MoveToObjectTween)}: Target не указан на {gameObject.name}");
                return DOTween.Sequence();
            }

            var currentAim = GetCurrentAimObject();
            if (currentAim == null)
            {
                Debug.LogWarning($"{nameof(MoveToObjectTween)}: AimObject не указан на {gameObject.name}");
                return DOTween.Sequence();
            }

            var worldPos = currentAim.position;
            Vector2 localPos;
            RectTransform parentRect = _target.parent as RectTransform;

            if (parentRect != null)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, worldPos, null, out localPos);
            else
                localPos = worldPos;

            return _target.DOAnchorPos(localPos, _duration).SetEase(_ease);
        }
    }
}
