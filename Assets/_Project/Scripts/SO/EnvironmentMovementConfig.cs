using System.Collections.Generic;
using _Project.Scripts.GameObjects;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(
        fileName = "EnvironmentMovementConfig",
        menuName = "SO/Environment Movement Config")]
    public class EnvironmentMovementConfig : ScriptableObject
    {
        [Header("Prefab")]
        [SerializeField] private EnvironmentElementController _prefab;

        [Header("Environment Sprites")]
        [SerializeField] private List<Sprite> _environmentSprites;

        [Header("Movement")]
        [SerializeField] private float _speed = 0.5f;
        [SerializeField] private float _turnOffset = 1f;

        [Header("Elements")]
        [SerializeField] private int _elementsCount = 10;

        [Header("Spawn")]
        [SerializeField] private Vector2 _spawnXRange = new(-5f, 5f);
        [SerializeField] private Vector2 _spawnYRange = new(-2f, 2f);
        [SerializeField] private float _spawnAboveCamera = 2f;

        [Header("Despawn")]
        [SerializeField] private float _despawnBelowCamera = 3f;

        public EnvironmentElementController Prefab => _prefab;

        public IReadOnlyList<Sprite> EnvironmentSprites => _environmentSprites;

        public float Speed => _speed;
        public float TurnOffset => _turnOffset;

        public int ElementsCount => _elementsCount;

        public Vector2 SpawnXRange => _spawnXRange;

        public Vector2 SpawnYRange => _spawnYRange;

        public float SpawnAboveCamera => _spawnAboveCamera;

        public float DespawnBelowCamera => _despawnBelowCamera;
    }
}