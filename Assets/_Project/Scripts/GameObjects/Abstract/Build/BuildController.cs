using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using _Project.Scripts.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects.Abstract.Build
{
    public abstract class BuildController<TModel, TView> : BuildController
        where TModel : BuildModel
        where TView : BuildView 
    {
        protected new TModel Model => (TModel)base.Model;
        protected new TView View => (TView)base.View;
    }
    
    public abstract class BuildController : ObjectController<BuildModel, BuildView>
    {
        [Inject] protected BuildPool BuildPool;

        public int BuildPrice => Model.BuildPrice;
        public BuildType BuildType => Model.BuildType;
        
        public override async UniTask Killed(Vector3 forceDirection, float forceAmount = 0f)
        {
            await UniTask.Delay(0);
            Dispose();
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