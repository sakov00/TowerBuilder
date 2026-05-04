using System;
using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TweenControllers
{
    public class TweenIfActiveEnableRaycast : TweenAction
    {
        [SerializeField] private GameObject _ifActiveObject;
        [SerializeField] private Graphic _active;
        [SerializeField] private Graphic _notActive;
        
        public override Tween GetTween()
        {
            var sequence = DOTween.Sequence();
            if (_ifActiveObject.activeInHierarchy)
                return sequence.AppendCallback(() =>
                {
                    _active.raycastTarget = true;
                });
            else
                return sequence.AppendCallback(() =>
                {
                    _notActive.raycastTarget = false;
                });
        }
    }
}