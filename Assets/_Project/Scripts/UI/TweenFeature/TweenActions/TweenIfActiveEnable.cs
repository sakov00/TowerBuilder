using System;
using System.Collections.Generic;
using _Project.Scripts.UI.TweenFeature.TweenActions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TweenControllers
{
    public class TweenIfActiveEnable : TweenAction
    {
        [SerializeField] private GameObject _ifActiveObject;
        [SerializeField] private GameObject _active;
        [SerializeField] private GameObject _notActive;
        
        public override Tween GetTween()
        {
            var sequence = DOTween.Sequence();
            if (_ifActiveObject.activeInHierarchy)
                return sequence.AppendCallback(() =>
                {
                    _active.SetActive(true);
                });
            else
                return sequence.AppendCallback(() =>
                {
                    _notActive.SetActive(false);
                });
        }
    }
}