using System.Collections.Generic;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class AttackAllLiveService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;
        
        private readonly List<IAttackable> _attackables = new();

        public void Tick()
        {
            _liveRegistry.GetAllByType(_attackables);

            for (int i = 0; i < _attackables.Count; i++)
            {
                HandleAttack(_attackables[i]);
            }
        }

        private void HandleAttack(IAttackable attackable)
        {
            if (attackable is not ISearchController searcher)
            {
                attackable.SetAttacking(false);
                return;
            }

            var target = searcher.CurrentAim;

            if (target == null)
            {
                attackable.SetAttacking(false);
                return;
            }

            Vector3 attackPoint = attackable.AttackPoint();
            float sqrDist = (attackPoint - attackable.Position).sqrMagnitude;
            float attackRangeSqr = attackable.AttackRange * attackable.AttackRange;

            attackable.SetAttacking(sqrDist <= attackRangeSqr);
        }
    }
}