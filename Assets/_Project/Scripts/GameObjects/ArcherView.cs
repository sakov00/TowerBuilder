using System;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Abstract.Unit;
using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    public class ArcherView : UnitView
    {
        [field: SerializeField] public ProjectileType ProjectileType { get; set; }
        [field: SerializeField] public Transform FirePoint { get; set; }
        [field: SerializeField] public float ProjectileSpeed { get; set; } = 40f;
    }
}