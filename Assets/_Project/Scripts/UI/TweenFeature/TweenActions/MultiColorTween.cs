using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TweenActions
{
    public class MultiColorTween : TweenAction
    {
        [Header("Targets")]
        [SerializeField] private List<Graphic> _targets = new List<Graphic>();

        [Header("Settings")]
        [SerializeField] private Color _to = Color.white;
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private Ease _ease = Ease.Linear;
        [SerializeField] private bool _canDisable = false;
        [SerializeField] private bool _yoyo = false;
        [SerializeField] private bool _autoAddChildren = false;

        private List<Color> _initialColors = new List<Color>();
        private Tween _tween;

        
        private void OnValidate()
        {
            if (_targets.Count == 0)
            {
                Debug.LogWarning($"{nameof(MultiColorTween)}: не назначен ни один целевой компонент на {gameObject.name}");
            }

                //TryAutoAddSelf();
                // TryAutoAddChildren();
            
        }
        
        private void TryAutoAddSelf()
        {
            var selfGraphic = GetComponent<Image>();
            if (selfGraphic != null && !_targets.Contains(selfGraphic))
            {
                _targets.Add(selfGraphic);
            }
        }

        private void TryAutoAddChildren()
        {
            if (!_autoAddChildren) return;

            var children = GetComponentsInChildren<Image>(true);

            foreach (var g in children)
            {
                if (!_targets.Contains(g))
                    _targets.Add(g);
            }
        }

        private void Awake()
        {
            _initialColors.Clear();
            foreach (var target in _targets)
            {
                if (target != null)
                    _initialColors.Add(target.color);
            }
        }

        public override Tween GetTween()
        {
            if (_targets.Count == 0)
            {
                Debug.LogWarning($"{nameof(ColorTween)}: не назначен ни один целевой компонент на {gameObject.name}");
                return DOTween.Sequence();
            }

            _tween?.Kill();

            if (_yoyo)
            {
                var sequence = DOTween.Sequence();
                for (int i = 0; i < _targets.Count; i++)
                {
                    var target = _targets[i];
                    if (target == null) continue;

                    sequence.Join(target.DOColor(_to, _duration).SetEase(_ease));
                }
                
                for (int i = 0; i < _targets.Count; i++)
                {
                    var target = _targets[i];
                    if (target == null) continue;

                    sequence.Append(target.DOColor(_initialColors[i], _duration).SetEase(_ease));
                }

                _tween = sequence;
            }
            else
            {
                var startValue = _targets.First().color;
                var sequence = DOTween.Sequence();
                if (_canDisable && startValue != Color.white)
                {
                    for (int i = 0; i < _targets.Count; i++)
                    {
                        var target = _targets[i];
                        if (target == null) continue;

                        sequence.Append(target.DOColor(_initialColors[i], _duration).SetEase(_ease));
                    }
                }
                else
                {
                    foreach (var target in _targets)
                    {
                        if (target == null) continue;
                        sequence.Join(target.DOColor(_to, _duration).SetEase(_ease));
                    }
                }

                _tween = sequence;
            }

            return _tween;
        }
    }
}
