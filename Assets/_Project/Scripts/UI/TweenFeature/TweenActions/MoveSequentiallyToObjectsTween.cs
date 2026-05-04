using System.Collections.Generic;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace UI.TweenActions
{
    public class MoveSequentiallyToObjectsTween : TweenAction
    {
        [SerializeField] private RectTransform _object;
        [SerializeField] private List<RectTransform> _targets = new List<RectTransform>();

        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
        [SerializeField] private float _delayBetween = 0.1f;

        [SerializeField] private bool _playOnAwake;

        private int countUse; 

        private void Awake()
        {
            if (_playOnAwake)
                GetTween().Play();
        }

        public override Tween GetTween()
        {
            if (_object == null)
            {
                Debug.LogWarning($"{nameof(MoveSequentiallyToObjectsTween)}: Target не указан на {gameObject.name}");
                return DOTween.Sequence();
            }

            var seq = DOTween.Sequence();

            if (_targets == null || _targets.Count == 0)
                return seq;

            // защита от выхода за границы
            if (countUse >= _targets.Count)
                countUse = 0;

            int foundIndex = -1;
            int startIndex = countUse;

            // ищем следующий полностью видимый объект (с циклом по списку)
            for (int i = 0; i < _targets.Count; i++)
            {
                int index = (startIndex + i) % _targets.Count;
                var aim = _targets[index];

                if (aim == null)
                    continue;

                if (!IsVisibleOnScreen(aim))
                    continue;

                foundIndex = index;
                break;
            }

            // если ни один объект не виден — выходим
            if (foundIndex == -1)
                return seq;

            var targetToMove = _targets[foundIndex];

            seq.Append(_object
                .DOMove(targetToMove.position, _duration)
                .SetEase(_ease));

            if (_delayBetween > 0)
                seq.AppendInterval(_delayBetween);

            countUse = (foundIndex + 1) % _targets.Count;

            return seq;
        }

        
        private bool IsVisibleOnScreen(RectTransform rect)
        {
            if (!rect.gameObject.activeInHierarchy)
                return false;

            var worldCorners = new Vector3[4];
            rect.GetWorldCorners(worldCorners);

            foreach (var corner in worldCorners)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(GlobalObjects.Camera, corner);

                if (screenPoint.x < 0 ||
                    screenPoint.y < 0 ||
                    screenPoint.x > Screen.width ||
                    screenPoint.y > Screen.height)
                {
                    // хотя бы один угол вне экрана → объект не полностью видим
                    return false;
                }
            }

            return true; // все углы внутри
        }
    }
}
