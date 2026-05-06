using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockDropService
    {
        [Inject] private LiveRegistry _liveRegistry;
        
        public void DropBlock()
        {
            var block = _liveRegistry.GetAllReactive()
                .OfType<BuildController>()
                .FirstOrDefault(b => b.Model.State == BuildState.Swinging);

            if (block == null)
                return;

            block.SetState(BuildState.Dropped);
            block.SetKinematicState(RigidbodyType2D.Dynamic);
        }
    }
}