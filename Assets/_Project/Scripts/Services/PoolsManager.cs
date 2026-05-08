using _Project.Scripts._VContainer;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Services
{
    public class PoolsManager : MonoBehaviour
    {
        [SerializeField] private Transform _buildPoolContainer;
        [SerializeField] private Transform _effectPoolContainer;
        
        [Inject] private BuildPool _buildPool;
        [Inject] private EffectPool _effectPool;

        private void Start()
        {
            InjectManager.Inject(this);
            _buildPool.SetContainer(_buildPoolContainer);
            _effectPool.SetContainer(_effectPoolContainer);
        }
    }
}