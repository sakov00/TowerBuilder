using System;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.GameObjects.Abstract.BaseObject
{
    [Serializable]
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(BuildModel))]
    public abstract partial class ObjectModel : ISavableModel
    {
        [MemoryPackInclude] public Vector3 SavePosition { get; set; }
        [MemoryPackInclude] public Quaternion SaveRotation { get; set; }
    }
}