using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.UI.Windows;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class BlockSpawnService
    {
        [Inject] private BuildPool _pool;
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private WindowsManager _windowsManager;

        public BuildController SpawnNext()
        {
            var gameWindow = _windowsManager.GetWindow<GameWindow>();
            var block = _pool.Get(BuildType.StartBlock, gameWindow.SpawnParent, gameWindow.SpawnPoint.position);

            block.SetState(BuildState.Swinging);
            block.SetKinematicState(RigidbodyType2D.Static);

            _liveRegistry.Register(block);

            return block;
        }
    }
}