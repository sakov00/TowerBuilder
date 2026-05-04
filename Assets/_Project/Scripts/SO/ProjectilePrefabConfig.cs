using System.Collections.Generic;
using _Project.Scripts.GameObjects.Additional.Projectiles;
using UnityEngine;

namespace _Project.Scripts.SO
{
    [CreateAssetMenu(fileName = "ProjectilePrefabConfig", menuName = "SO/Projectile Prefab Config")]
    public class ProjectilePrefabConfig : ScriptableObject
    {
        [Header("Projectile Prefabs")]
        public List<Projectile> allProjectiles;
    }
}