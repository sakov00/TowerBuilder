using System.Collections.Generic;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.GameObjects.Abstract.Unit
{
    public class UnitView : ObjectView
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsAttack = Animator.StringToHash("IsAttack");
        private static readonly int Rotate = Animator.StringToHash("Rotate");

        [SerializeField] private Animator _animator;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _shadowTransform;
        [SerializeField] private List<Rigidbody> _allRigidbodies;
        
        public Transform ShadowTransform => _shadowTransform;
        public bool IsMoving => _animator != null && _animator.GetBool(IsWalking);
        
        public override void Initialize()
        {
            base.Initialize();
            RagdollIsActive(false);
        }

        public void RagdollIsActive(bool isActive, Vector3? forceDirection = null, float forceAmount = 0f)
        {
            if (_animator == null) return;
            _agent.enabled = !isActive;
            _animator.enabled = !isActive;
            _allRigidbodies.ForEach(rigidbody =>
            {
                rigidbody.isKinematic = !isActive;
                
                if (isActive && forceDirection.HasValue)
                {
                    rigidbody.AddForce(forceDirection.Value.normalized * forceAmount, ForceMode.Impulse);
                }
            });
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
                RotateTowards(position);
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
        
        private void RotateTowards(Vector3 targetPos)
        {
            Vector3 direction = (targetPos - _transform.position).normalized;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _transform.rotation = Quaternion.Slerp(
                    _transform.rotation,
                    lookRotation,
                    Time.deltaTime * 10f // скорость поворота
                );
            }
        }
    }
}
