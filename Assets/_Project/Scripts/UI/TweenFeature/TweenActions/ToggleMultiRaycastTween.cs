using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TweenActions
{
    public class ToggleMultiRaycastTween : TweenAction
    {
        [SerializeField] private List<Graphic> _enableTargets;
        [SerializeField] private List<Graphic> _disableTargets;
        
        [SerializeField] private List<Graphics> _removeTargets;

        private void OnValidate()
        {
            if ((_enableTargets == null || _enableTargets.Count == 0) &&
                (_disableTargets == null || _disableTargets.Count == 0))
            {
                Debug.LogWarning($"{nameof(ToggleMultiRaycastTween)}: Нет объектов для включения или отключения на {gameObject.name}");
            }
        }

        public override Tween GetTween()
        {
            var tween = DOTween.Sequence();

            tween.AppendCallback(() =>
            {
                if (_enableTargets != null)
                {
                    foreach (var target in _enableTargets)
                    {
                        if (target != null) target.raycastTarget = true;
                    }
                }

                if (_disableTargets != null)
                {
                    foreach (var target in _disableTargets)
                    {
                        if (target != null) target.raycastTarget = false;
                    }
                }
            });

            return tween;
        }

        public void PlayTween()
        {
            GetTween().Play();
        }
        
        public override void RemoveAll(int numberStep)
        {
            _enableTargets.RemoveAll(_removeTargets[numberStep].graphics.Contains);
            _disableTargets.RemoveAll(_removeTargets[numberStep].graphics.Contains);
        }
    }

    [System.Serializable]
    public class Graphics
    {
        public List<Graphic> graphics;
    }
}