using System;
using MemoryPack;
using UnityEngine;
using BuildModel = _Project.Scripts.GameObjects.Abstract.Build.BuildModel;

namespace _Project.Scripts.GameObjects
{
    [Serializable]
    [MemoryPackable]
    public partial class FriendsBuildModel : BuildModel
    {
        [Header("Units")] 
        [MemoryPackIgnore][field:SerializeField] public int TimeCreateUnits { get; set; } = 5;
        [MemoryPackInclude][field:SerializeField] public int FriendsGroupId { get; set; }
        [MemoryPackInclude][field:SerializeField] public int NeedRestoreUnitsCount { get; set; }
        [MemoryPackInclude][field:SerializeField] public int CurrentSpawnTimer { get; set; } = -1;
    }
}