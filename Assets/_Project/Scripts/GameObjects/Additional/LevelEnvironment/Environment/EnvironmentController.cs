using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects.Additional.LevelEnvironment.Environment
{
    public class EnvironmentController : MonoBehaviour, ISavableController, IDestroyable
    {
        [Inject] private AppData _appData;
        [Inject] private SaveRegistry _saveRegistry;
        [SerializeField] protected EnvironmentModel _model;
        
        private void Awake()
        {
            InjectManager.Inject(this);
        }
        
        public void Initialize()
        {
            _saveRegistry.Register(this);
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