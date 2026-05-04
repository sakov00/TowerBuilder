using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.Projectiles;
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

        protected virtual void FixedUpdate()
        {
            // View.UpdateHealthBar(Model.CurrentHealth, Model.MaxHealth);
        }
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

        public WarSide WarSide => Model.WarSide;
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
        
        public virtual void TakeDamage(float damageAmount, Vector3 forceDirection = default, float forceAmount = 0f)
        {
            Model.CurrentHealth -= damageAmount;

            if (Model.CurrentHealth < 0)
                Model.CurrentHealth = 0;

            if (Model.CurrentHealth <= 0)
            {
                Killed(forceDirection, forceAmount).Forget();
            }
        }

        public Vector3 GetOwnAttackPoint(Vector3 fromPosition) => View.GetAttackPoint(fromPosition);

        public abstract UniTask Killed(Vector3 forceDirection = default, float forceAmount = 0f);
        public abstract void Dispose(bool returnToPool = true, bool clearFromRegistry = true);
    }
}