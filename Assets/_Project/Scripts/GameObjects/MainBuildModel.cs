using System;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;
using BuildModel = _Project.Scripts.GameObjects.Abstract.Build.BuildModel;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class MainBuildModel : BuildModel
    {
        [field: Header("Attack")] 
        [MemoryPackIgnore][field: SerializeField] public float AttackRange { get; set; } = 10f;
        [MemoryPackIgnore][field: SerializeField] public float DamageAmount { get; set; } = 10;
        [MemoryPackIgnore][field: SerializeField] public float AllAnimAttackTime { get; set; } = 1f;
        [MemoryPackIgnore][field: SerializeField] public float AnimAttackTime { get; set; } = 1f;
        [MemoryPackIgnore][field: SerializeField] public float DetectionRadius { get; set; } = 20f;
        [MemoryPackIgnore][field: SerializeField] public ObjectController AimObject { get; set; }
    }
}