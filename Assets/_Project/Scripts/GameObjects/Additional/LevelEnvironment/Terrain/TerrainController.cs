using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Additional.LevelEnvironment.Terrain
{
    public class TerrainController : MonoBehaviour, ISavableController, IDestroyable
    {
        [Inject] private AppData _appData;
        [Inject] private SaveRegistry _saveRegistry;
        
        [SerializeField] private TerrainModel _model;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private NavMeshSurface _meshSurface;
        
        private void Awake()
        {
            InjectManager.Inject(this);
        }
        
        public void Initialize()
        {
            _saveRegistry.Register(this);
            ChangeTerrain().Forget();
        }

        public ISavableModel GetSavableModel()
        {
            _model.SavePosition = transform.position;
            _model.SaveRotation = transform.rotation;
            return _model;
        }

        public void SetSavableModel(ISavableModel savableModel) =>
            _model.LoadData(savableModel);

        private async UniTask ChangeTerrain()
        {
            await UniTask.Yield();
            _meshSurface.BuildNavMesh();
        }

        private float GetAreaValue()
        {
            var worldSize = Vector3.Scale(_meshFilter.sharedMesh.bounds.size, transform.lossyScale);
            var groundArea = worldSize.x * worldSize.z;
            return groundArea;
        }

        public void Destroy() => Destroy(gameObject);

        private void OnDestroy()
        {
            _saveRegistry.Unregister(this);
        }
    }
}