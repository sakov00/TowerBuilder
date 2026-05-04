using System;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.UI.Info;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    public class PlayerView : ObjectView
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        private static readonly int Rotate = Animator.StringToHash("Rotate");

        [SerializeField] private Animator _animator;
        [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;
        [SerializeField] private Transform _shadowTransform;
        
        public Transform ShadowTransform => _shadowTransform;
        public bool IsMoving => _animator != null && _animator.GetBool(IsWalking);
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public virtual void SetWalking(bool isWalking)
        {
            if (_animator == null) return;
            if (_animator.GetBool(IsWalking) == isWalking)
                return;
            
            _animator.SetBool(IsWalking, isWalking);
        }

        public virtual void SetAttacking(bool isAttacking)
        {
            if (_animator == null) return;
            if (_animator.GetBool(IsAttack) == isAttacking)
                return;

            SetWalking(false);
            _animator.SetBool(IsAttack, isAttacking);
        }
        
        public virtual void SetRotate()
        {
            if (_animator == null) return;
            _animator.SetTrigger(Rotate);
        }
        
        public void Select()
        {
            EnableOutline(true);
        }

        public void Deselect()
        {
            EnableOutline(false);
        }

        public void MoveTo(Vector3 position)
        {
            if (!_agent.enabled)
                return;
            
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                Stop();
            }

            _agent.isStopped = false;
            _agent.SetDestination(position);

            SetWalking(true);
        }
        
        public void Stop()
        {
            if (!_agent.enabled)
                return;

            _agent.isStopped = true;
            _agent.ResetPath();

            SetWalking(false);
        }
    }
}