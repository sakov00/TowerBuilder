using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Registries;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    public abstract class ObjectController<TModel, TView> : ObjectController
        where TModel : ObjectModel
        where TView : ObjectView 
    {
        protected new TModel Model => (TModel)base.Model;
        protected new TView View => (TView)base.View;
    }

    public abstract class ObjectController : MonoBehaviour, ISavableController, IPoolableDispose
    {
        [Inject] protected AppData AppData;
        [Inject] protected LiveRegistry LiveRegistry;
        [Inject] protected SaveRegistry SaveRegistry;
        
        [SerializeReference, SubclassSelector]
        private ObjectModel _model;
        
        [SerializeReference, SubclassSelector]
        private ObjectView _view;

        public bool IsVisible => View.IsVisible;

        public ObjectModel Model
        {
            get { return _model; } 
            set { _model = value; }
        }
    
        public ObjectView View
        {
            get { return _view; } 
            set { _view = value; }
        }
    
        protected virtual void Awake()
        {
            InjectManager.Inject(this);
            Initialize();
        }
        
        public virtual void Initialize()
        {
            LiveRegistry.Register(this);
            SaveRegistry.Register(this);
            Dispose(false, false);
        }
        
        private void OnDestroy()
        {
            Dispose(false);
        }
        
        public ISavableModel GetSavableModel() => _model;
        public void SetSavableModel(ISavableModel savableModel)
        {
            _model.SavePosition = transform.position;
            _model.SaveRotation = transform.rotation;
            _model = (ObjectModel)savableModel;
        }

        public abstract void Dispose(bool returnToPool = true, bool clearFromRegistry = true);
        
        public async UniTaskVoid DisposeDelayed(bool returnToPool = true, bool clearFromRegistry = true)
        {
            await UniTask.Delay(2000);
            Dispose(returnToPool, clearFromRegistry);
        }
    }
}