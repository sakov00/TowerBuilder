using _Project.Scripts.GameObjects;
using _Project.Scripts.GameObjects.Abstract.BaseObject;
using MemoryPack;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(ObjectModel))]
    [MemoryPackUnion(1, typeof(BuildModel))]
    public partial interface ISavableModel
    {
        public Vector3 SavePosition { get; set; }
        public Quaternion SaveRotation { get; set; }
    }
}