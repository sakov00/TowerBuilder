using System.Linq;
using UnityEngine;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using _Project.Scripts.UI.Windows;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class GameZoneFollowService : ITickable
    {
        [Inject] private WindowsManager _windowsManager;
        [Inject] private LiveRegistry _liveRegistry;

        private float _smoothSpeed = 5f;
        private float _offsetMoveY = -300f;

        private float _targetY;
        private BuildController _lastHighest;

        public void Tick()
        {
            var blocks = _liveRegistry.GetAllReactive();

            var highest = blocks
                .OfType<BuildController>()
                .Where(b => b.Model.State == Enums.BuildState.Placed)
                .OrderBy(b => b.transform.position.y)
                .LastOrDefault();

            if (highest == null)
                return;

            if (_lastHighest != highest)
            {
                _lastHighest = highest;
                _targetY = highest.RectTransform.anchoredPosition.y + _offsetMoveY;
            }

            var gameZone = _windowsManager.GetWindow<GameWindow>().GameZone;

            Vector2 pos = gameZone.anchoredPosition;

            pos.y = Mathf.Lerp(pos.y, _targetY, Time.deltaTime * _smoothSpeed);

            gameZone.anchoredPosition = pos;
        }
    }
}