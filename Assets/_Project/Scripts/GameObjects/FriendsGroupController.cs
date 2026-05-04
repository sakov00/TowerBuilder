using System;
using _Project.Scripts._VContainer;
using _Project.Scripts.AllAppData;
using _Project.Scripts.GameObjects.Abstract.Unit;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using _Project.Scripts.Registries;
using UniRx;
using UnityEngine;
using VContainer;
using ISavableModel = _Project.Scripts.Interfaces.ISavableModel;

namespace _Project.Scripts.GameObjects
{

    public class FriendsGroupController : MonoBehaviour, ISavableController, IPoolableDispose, ISelectable
    {
        [Inject] private AppData _appData;
        [Inject] private SaveRegistry _saveRegistry;
        [Inject] private UnitPool _unitPool;
        
        [field:SerializeField] public FriendsGroupModel Model { get; private set;}
        [field:SerializeField] public FriendsGroupView View { get; private set;}
        
        public Action<UnitController> UnitOnKilled;
        
        public int Id { get => Model.Id; set => Model.Id = value; }
        public ReactiveCollection<UnitController> Units { get; set; } = new();
        
        private CompositeDisposable _disposables;

        public void Initialize()
        {
            InjectManager.Inject(this);
            _saveRegistry.Register(this);
            
            _disposables = new CompositeDisposable();

            Units.ObserveAdd().Subscribe(addedUnit =>
            {
                // Model.UnitIds.Add(addedUnit.Value.Id);
                // addedUnit.Value.OnKilled += OnKilledHandler;
            }).AddTo(_disposables);
            
            Units.ObserveRemove().Subscribe(removedUnit =>
            {
                // Model.UnitIds.Remove(removedUnit.Value.Id);
                UnitOnKilled?.Invoke(removedUnit.Value);
                // removedUnit.Value.OnKilled -= OnKilledHandler;
            }).AddTo(_disposables);
            
            AddFriends();
        }
        
        private void OnKilledHandler(UnitController removedUnit)
        {
            Units.Remove(removedUnit);
        }
        
        private void AddFriends()
        {
            if (Model.UnitIds.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    var unitController = _unitPool.Get(Model.UnitType, transform.position);
                    // unitController.SetWayToPoint(new List<Vector3> { transform.position });
                    unitController.Initialize();
                    Units.Add(unitController);
                }

                View.ArrangeUnitsInRadius(Units, Model.GroupRadius);
            }
            else
            {
                foreach (var unitId in Model.UnitIds)
                {
                    // var unit = (UnitController)_idsRegistry.Get(unitId);
                    // Units.Add(unit);
                }
            }
        }
        
        public void Select()
        {
            // foreach (var unit in Units)
            //     unit.Select();
        }
        
        public void Deselect()
        {
            // foreach (var unit in Units)
            //     unit.Deselect();
        }

        public void MoveTo(Vector3 position)
        {
            transform.position = position;
            View.ArrangeUnitsInRadius(Units, Model.GroupRadius);
        }

        public ISavableModel GetSavableModel()
        {
            Model.SavePosition = transform.position;
            Model.SaveRotation = transform.rotation;
            return Model;
        }

        public void SetSavableModel(ISavableModel savableModel) =>
            Model.LoadData(savableModel);
        
        public void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            _disposables?.Dispose();
            UnitOnKilled = null;
            foreach (var unit in Units) 
                // unit.OnKilled -= OnKilledHandler;
            // if(returnToPool) BuildPool.Return(this);
            if (clearFromRegistry)
            {
                _saveRegistry.Unregister(this);
            }
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Dispose(false);
        }
    }
}