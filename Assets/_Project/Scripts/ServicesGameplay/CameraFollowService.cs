using System.Linq;
using _Project.Scripts._GlobalLogic;
using _Project.Scripts.AllAppData;
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
        private readonly LiveRegistry _liveRegistry;
        private readonly CameraConfig _cameraConfig;
        private readonly AppData _appData;

        [Inject]
        public CameraFollowService(LiveRegistry liveRegistry, CameraConfig cameraConfig, AppData appData)
        {
            _liveRegistry = liveRegistry;
            _cameraConfig = cameraConfig;
            _appData = appData;
        }

        public void Tick()
        {
            if(_appData.LevelData.GameDisabled)
                return;
            
            var blocks = _liveRegistry.GetAllReactive();

            var highest = blocks
                .OfType<BuildController>()
                .Where(b => b.Model.State == Enums.BuildState.Placed)
                .OrderBy(b => b.transform.position.y)
                .LastOrDefault();

            if (highest == null)
                return;

            if (_appData.LevelData.HighestBlock != highest)
            {
                _appData.LevelData.HighestBlock = highest;
            }
            
            Vector3 pos = GlobalObjects.Camera.transform.position;
            
            pos.y = Mathf.Lerp(pos.y, highest.Transform.position.y + _cameraConfig.OffsetMoveY, Time.deltaTime * _cameraConfig.SmoothSpeed);
            pos.y = Mathf.Clamp(pos.y, _cameraConfig.MinY, _cameraConfig.MaxY);
            GlobalObjects.Camera.transform.position = pos;
        }

        public void Reset()
        {
            GlobalObjects.Camera.transform.position = new Vector3(0, _cameraConfig.MinY, -10);
        }
    }
}