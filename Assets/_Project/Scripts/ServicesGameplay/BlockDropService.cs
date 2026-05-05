using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockDropService : ITickable
    {
        private readonly LiveRegistry _liveRegistry;

        public BlockDropService(LiveRegistry liveRegistry)
        {
            _liveRegistry = liveRegistry;
        }

        public void Tick()
        {
            var block = _liveRegistry.GetAllReactive()
                .OfType<BuildController>()
                .FirstOrDefault(b => b.Model.State == BuildState.Swinging);

            if (block == null)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                block.SetState(BuildState.Dropped);
                block.SetKinematicState(RigidbodyType2D.Dynamic);
            }
        }
    }
}