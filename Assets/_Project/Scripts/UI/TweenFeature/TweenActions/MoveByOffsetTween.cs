using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace TweenActions
{
    public enum MoveLogic
    {
        FromOffsetToCurrent,
        FromCurrentToOffset,
        PingPong,
        FromOffsetToOffset
    }

    public enum Axis
    {
        Horizontal,
        Vertical,
        Both
    }

    public class MoveByOffsetTween : TweenAction
    {
        [SerializeField] private RectTransform _target;

        [Header("Offsets")]
        [SerializeField] private Vector2 _offset;

        [Header("Tween Settings")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [SerializeField] private bool _playOnAwake;
        [SerializeField] private bool _checkState;

        [Header("Move Logic")]
        [SerializeField] private MoveLogic _moveLogic = MoveLogic.FromOffsetToCurrent;

        [Header("Move Axis")]
        [SerializeField] private Axis _axis = Axis.Horizontal;

        private bool _lastIsPortrait;

        private void OnValidate()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(MoveByOffsetTween)}: Target не найден на {gameObject.name}");
            }
        }

        private void Awake()
        {
            _lastIsPortrait = Screen.height > Screen.width;

            if (_playOnAwake)
            {
                if (IsAxisActiveForOrientation(_lastIsPortrait))
                {
                    GetTween().Play();
                }
            }
        }

        private void Update()
        {
            if (!_checkState)
                return;

            bool isPortrait = Screen.height > Screen.width;

            if (isPortrait != _lastIsPortrait)
            {
                _lastIsPortrait = isPortrait;

                if (IsAxisActiveForOrientation(isPortrait))
                {
                    GetTween().Play();
                }
            }
        }

        private bool IsAxisActiveForOrientation(bool isPortrait)
        {
            if (_axis == Axis.Both)
                return true;

            if (_axis == Axis.Horizontal)
                return !isPortrait;

            if (_axis == Axis.Vertical)
                return isPortrait;

            return false;
        }

        private Vector2 ApplyAxis(Vector2 original, Vector2 offset)
        {
            if (_axis == Axis.Horizontal)
            {
                return new Vector2(original.x + offset.x, original.y);
            }
            else if (_axis == Axis.Vertical)
            {
                return new Vector2(original.x, original.y + offset.y);
            }
            else if (_axis == Axis.Both)
            {
                return new Vector2(original.x + offset.x, original.y + offset.y);
            }

            return original;
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(MoveByOffsetTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }

            Vector2 currentPos = _target.anchoredPosition;

            switch (_moveLogic)
            {
                case MoveLogic.FromOffsetToCurrent:
                {
                    Vector2 startPos = ApplyAxis(currentPos, _offset);
                    _target.anchoredPosition = startPos;
                    return _target.DOAnchorPos(currentPos, _duration).SetEase(_ease);
                }

                case MoveLogic.FromCurrentToOffset:
                {
                    Vector2 endPos = ApplyAxis(currentPos, _offset);
                    return _target.DOAnchorPos(endPos, _duration).SetEase(_ease);
                }

                case MoveLogic.PingPong:
                {
                    Vector2 startPos = ApplyAxis(currentPos, _offset);
                    _target.anchoredPosition = startPos;
                    return _target.DOAnchorPos(currentPos, _duration)
                        .SetEase(_ease)
                        .SetLoops(2, LoopType.Yoyo);
                }

                case MoveLogic.FromOffsetToOffset:
                {
                    Vector2 startPos = ApplyAxis(currentPos, _offset);
                    Vector2 endPos = ApplyAxis(currentPos, -_offset);
                    _target.anchoredPosition = startPos;
                    return _target.DOAnchorPos(endPos, _duration).SetEase(_ease);
                }

                default:
                    return DOTween.Sequence();
            }
        }
    }
}
