using System.Collections.Generic;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class MoveAllMovablesService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;
        
        private readonly List<IMovable> _movables = new();

        public void Tick()
        {
            _liveRegistry.GetAllByType(_movables);

            for (int i = 0; i < _movables.Count; i++)
            {
                MoveSingle(_movables[i]);
            }
        }

        private void MoveSingle(IMovable movable)
        {
            if (movable is not ISearchController searcher)
            {
                if (movable.IsMoving)
                    movable.Stop();
                return;
            }

            if (searcher.CurrentAim == null)
            {
                if (movable.IsMoving)
                    movable.Stop();
                return;
            }
            
            movable.MoveTo(searcher.CurrentAim.GetOwnAttackPoint(movable.Position));
        }
    }
}