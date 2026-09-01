using _Project.Scripts.Enums;
using _Project.Scripts.ServicesGameplay;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class DeadZone : MonoBehaviour
    {
        [Inject] private BlockPlacementService _placementService;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            if (!other.CompareTag("Block"))
                return;

            var block = other.GetComponent<BuildController>();
            if (block.Model.State != BuildState.Dropped)
                return;

            _placementService.Resolve(block).Forget();
        }
    }
}