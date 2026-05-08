using System.Linq;
using _Project.Scripts._GlobalLogic;
using UnityEngine;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using _Project.Scripts.UI.Windows;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.ServicesGameplay
{
    public class CameraFollowService : ITickable
    {
        [Inject] private LiveRegistry _liveRegistry;
        [Inject] private CameraConfig _cameraConfig;

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
                _targetY = highest.Transform.position.y + _cameraConfig.OffsetMoveY;
            }
            
            Vector3 pos = GlobalObjects.Camera.transform.position;
            
            pos.y = Mathf.Lerp(pos.y, _targetY + 0.5f, Time.deltaTime * _cameraConfig.SmoothSpeed);
            pos.y = Mathf.Clamp(pos.y, _cameraConfig.MinY, _cameraConfig.MaxY);
            GlobalObjects.Camera.transform.position = pos;
        }
    }
}