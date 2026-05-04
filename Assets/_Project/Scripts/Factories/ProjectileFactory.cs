using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.GameObjects.Additional.Projectiles;
using _Project.Scripts.SO;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Factories
{
    public class ProjectileFactory
    {
        [Inject] private IObjectResolver _resolver;
        [Inject] private ProjectilePrefabConfig _prefabConfig;
        
        public Projectile CreateProjectile(ProjectileType type, Vector3 position, Quaternion rotation = default)
        {
            var prefab = _prefabConfig.allProjectiles.FirstOrDefault(p => p.ProjectileType == type);

            if (prefab == null)
            {
                Debug.LogError($"ProjectileFactory: Prefab with ProjectileType {type} not found in config");
                return null;
            }

            return _resolver.Instantiate(prefab, position, rotation);
        }
    }
}