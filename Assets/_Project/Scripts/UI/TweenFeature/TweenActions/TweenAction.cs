using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.UI.TweenFeature.TweenActions
{
    public abstract class TweenAction : MonoBehaviour
    {
        public abstract Tween GetTween();
        
        public virtual void Dispose()
        {}
        
        public virtual void RemoveAll(int numberStep) {}
    }
}