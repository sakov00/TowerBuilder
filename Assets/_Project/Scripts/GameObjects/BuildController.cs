using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Pools;
using _Project.Scripts.ServicesGameplay;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class BuildController : ObjectController<BuildModel, BuildView>
    {
        [Inject] protected BuildPool BuildPool;
        [Inject] private BlockPlacementService _placementService;
        public new BuildModel Model => base.Model;
        public new BuildView View => base.View;

        public Transform Transform => View.Transform;
        
        public void SetKinematicState(RigidbodyType2D state) => View.SetKinematicState(state);
        public void MoveRigidbody(Vector3 targetPos) => View.MoveRigidbody(targetPos);
        public void SetImage(Sprite sprite) => View.SetImage(sprite);
        public void SetState(BuildState state)
        {
            Model.State = state;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (Model.State != BuildState.Dropped)
                return;

            if (collision.gameObject.TryGetComponent<BuildController>(out var other))
            {
                _placementService.Resolve(this, other);
            }
        }
        
        public override void Dispose(bool returnToPool = true, bool clearFromRegistry = true)
        {
            if(returnToPool) BuildPool.Return(this);
            if (clearFromRegistry)
            {
                LiveRegistry.Unregister(this);
                SaveRegistry.Unregister(this);
            }
        }
    }
}