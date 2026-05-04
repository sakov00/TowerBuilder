using System;
using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;

namespace TweenControllers
{
    public class TweenIfActive : TweenAction
    {
        [SerializeField] private GameObject _ifActiveObject;
        [SerializeField] private TweenAction _activeTween;
        [SerializeField] private TweenAction _notActiveObject;
        
        public override Tween GetTween()
        {
            if(_ifActiveObject.activeInHierarchy)
                return _activeTween.GetTween();
            else
                return _notActiveObject.GetTween();
        }

        public override void Dispose()
        {
            if(_ifActiveObject.activeInHierarchy)
                _activeTween.Dispose();
            else
                _notActiveObject.Dispose();
        }
    }
}