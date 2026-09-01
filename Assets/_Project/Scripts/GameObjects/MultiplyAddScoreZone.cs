using System;
using _Project.Scripts.AllAppData;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class MultiplyAddScoreZone : MonoBehaviour
    {
        [Inject] protected AppData _appData;

        [SerializeField] private BoxCollider2D _collider;

        private void OnValidate()
        {
            _collider ??= GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            if (!other.CompareTag("Block"))
                return;

            _appData.LevelData.AddScoreValue *= 2;
            _collider.enabled = false;
            enabled = false;
        }
    }
}