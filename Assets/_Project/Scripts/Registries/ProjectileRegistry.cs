using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.GameObjects.Additional.Projectiles;
using Unity.VisualScripting;

namespace _Project.Scripts.Registries
{
    public class ProjectileRegistry
    {
        private readonly HashSet<Projectile> _projectiles = new();

        public void Register(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }

        public void RegisterRange(List<Projectile> projectiles)
        {
            _projectiles.AddRange(projectiles);
        }

        public void Unregister(Projectile projectile)
        {
            _projectiles.Remove(projectile);
        }

        public List<Projectile> GetAllByType()
        {
            return _projectiles.ToList();
        }

        public IReadOnlyCollection<Projectile> GetAll()
        {
            return _projectiles;
        }

        public void Clear()
        {
            _projectiles.Clear();
        }
    }
}