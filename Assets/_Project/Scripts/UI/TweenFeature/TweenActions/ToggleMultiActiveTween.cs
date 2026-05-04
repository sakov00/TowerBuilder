using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace TweenActions
{
    public class ToggleMultiActiveTween : TweenAction
    {
        [SerializeField] private bool _canDisable = false;
        [SerializeField] private List<GameObject> _enableTargets;
        [SerializeField] private List<GameObject> _disableTargets;
        
        [SerializeField] private List<GameObjects> _removeTargets;

        private void OnValidate()
        {
            if ((_enableTargets == null || _enableTargets.Count == 0) &&
                (_disableTargets == null || _disableTargets.Count == 0))
            {
                Debug.LogWarning($"{nameof(ToggleMultiActiveTween)}: Нет объектов для включения или отключения на {gameObject.name}");
            }
            //
            // _enableTargets.RemoveAll(x => x == null);
            // _disableTargets.RemoveAll(x => x == null);
        }

        public override Tween GetTween()
        {
            var tween = DOTween.Sequence();

            tween.AppendCallback(() =>
            {
                var isEnableTargets = _enableTargets.FirstOrDefault()?.activeInHierarchy;
                    
                if (_disableTargets != null)
                {
                    foreach (var target in _disableTargets)
                    {
                        if (target != null) target.SetActive(false);
                    }
                }
                
                if (_enableTargets != null)
                {
                    foreach (var target in _enableTargets)
                    {
                        if (target != null)
                        {
                            if(_canDisable && isEnableTargets.HasValue)
                                target.SetActive(!isEnableTargets.Value);
                            else
                                target.SetActive(true);
                        }
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
            _enableTargets.RemoveAll(_removeTargets[numberStep].gameObjects.Contains);
            _disableTargets.RemoveAll(_removeTargets[numberStep].gameObjects.Contains);
        }
    }

    [System.Serializable]
    public class GameObjects
    {
        public List<GameObject> gameObjects;
    }
}