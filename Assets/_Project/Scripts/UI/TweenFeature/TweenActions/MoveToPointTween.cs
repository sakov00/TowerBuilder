using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class MoveToPointTween : TweenAction
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private Vector3 _moveTo;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        [Header("Orientation Settings")]
        [SerializeField] private bool _playInPortrait = true;
        [SerializeField] private bool _playInLandscape = true;
        
        private void OnValidate()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(MoveToPointTween)}: Target не найден на {gameObject.name}");
            }
        }

        public override Tween GetTween()
        {
            if (_target == null)
            {
                Debug.LogWarning($"{nameof(MoveToPointTween)}: Target не найден на {gameObject.name}");
                return DOTween.Sequence();
            }
            
            bool isPortrait = Screen.height > Screen.width;

            if ((isPortrait && !_playInPortrait) || (!isPortrait && !_playInLandscape))
                return DOTween.Sequence();

            return _target.DOAnchorPos(_moveTo, _duration).SetEase(_ease);
        }
        
        public void Play() => GetTween().Play();
    }
}