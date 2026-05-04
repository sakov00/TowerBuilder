using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects.Additional.Projectiles;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Pools
{
    public class ProjectilePool
    {
        [Inject] private ProjectileFactory _projectileFactory;
        
        private Transform _containerTransform;
        private readonly List<Projectile> _availableProjectiles = new();

        public void SetContainer(Transform transform)
        {
            _containerTransform = transform;
        }
        
        public Projectile Get(ProjectileType projectileType, Vector3 position = default, Quaternion rotation = default) 
        {
            var projectile = _availableProjectiles.FirstOrDefault(c => c.ProjectileType == projectileType);
            if (projectile != null)
            {
                _availableProjectiles.Remove(projectile);
                projectile.transform.position = position;
                projectile.transform.rotation = rotation;
                projectile.gameObject.SetActive(true);
            }
            else
            {
                projectile = _projectileFactory.CreateProjectile(projectileType, position, rotation);
            }

            projectile.transform.SetParent(null);
            return projectile;
        }

        public void Return(Projectile projectile)
        {
            if (!_availableProjectiles.Contains(projectile))
            {
                _availableProjectiles.Add(projectile);
            }
            
            projectile.gameObject.SetActive(false);
            projectile.transform.SetParent(_containerTransform, false); 
        }
    }
}