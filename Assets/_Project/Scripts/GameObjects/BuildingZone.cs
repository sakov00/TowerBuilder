using System.Linq;
using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using _Project.Scripts.SO;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{
    public class BuildingZone : MonoBehaviour, IBuy, ISavableController, IDestroyable
    {
        [Inject] private AppData _appData;
        [Inject] private BuildingPrefabConfig _buildingPrefabConfig;
        [Inject] private BuildPool _buildPool;
        [Inject] private SaveRegistry _saveRegistry;

        [SerializeField] public BuildingZoneModel _model;
        
        private Vector3 _originalScale;

        private void Awake()
        {
            InjectManager.Inject(this);
        }

        public void Initialize()
        {
            _saveRegistry.Register(this);
        }

        public async UniTask TryBuy()
        {
            if (_originalScale == Vector3.zero)
                _originalScale = transform.localScale;

            var reducedScale = _originalScale * 0.9f;

            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(reducedScale, 0.25f));
            sequence.Append(transform.DOScale(_originalScale, 0.25f));
            await sequence.Play();

            var prefab = _buildingPrefabConfig.allBuildPrefabs.First(p => p.BuildType == _model.BuildType);
            var price = prefab.BuildPrice;
            if (price > _appData.LevelData.LevelMoney)
            {
                Debug.Log("Not enough money");
                return;
            }

            _appData.LevelData.LevelMoney -= price;
            var build = _buildPool.Get(_model.BuildType, transform.position, transform.rotation);
            build.Initialize();
            Destroy();
        }

        public ISavableModel GetSavableModel()
        {
            _model.SavePosition = transform.position;
            _model.SaveRotation = transform.rotation;
            return _model;
        }

        public void SetSavableModel(ISavableModel savableModel) =>
            _model.LoadData(savableModel);

        public void Destroy() => Destroy(gameObject);

        private void OnDestroy()
        {
            _saveRegistry.Unregister(this);
        }
    }
}