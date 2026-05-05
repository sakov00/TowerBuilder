using _Project.Scripts._VContainer;
using _Project.Scripts.Pools;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Services
{
    public class PoolsManager : MonoBehaviour
    {
        [SerializeField] private Transform _buildPoolContainer;
        
        [Inject] private BuildPool _buildPool;

        private void Start()
        {
            InjectManager.Inject(this);
            _buildPool.SetContainer(_buildPoolContainer);
        }
    }
}