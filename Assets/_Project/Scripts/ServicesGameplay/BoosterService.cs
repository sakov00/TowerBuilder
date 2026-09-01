using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.ServicesGameplay
{
    public class BoosterService
    {
        private readonly AppData _appData;
        private readonly LiveRegistry _liveRegistry;

        [Inject]
        public BoosterService(AppData appData, LiveRegistry liveRegistry)
        {
            _appData = appData;
            _liveRegistry = liveRegistry;
        }        
        
        public void ResetBalance()
        {
            _appData.LevelData.TotalSwayImbalance = 0;
            _appData.LevelData.Health = 3;
            var blocks = _liveRegistry.GetAllReactive();
            foreach (var block in blocks)
            {
                block.transform.localPosition = new Vector3(0, block.transform.localPosition.y, block.transform.localPosition.z);
            }
        }
    }
}