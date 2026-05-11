using _Project.Scripts._VContainer;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Services
{
    public class PoolsManager : MonoBehaviour, IStartable
    {
        [SerializeField] private Transform _buildPoolContainer;
        [SerializeField] private Transform _effectPoolContainer;
        
        [Inject] private BuildPool _buildPool;
        [Inject] private EffectPool _effectPool;

        public void Start()
        {
            _buildPool.SetContainer(_buildPoolContainer);
            _effectPool.SetContainer(_effectPoolContainer);
        }
    }
}