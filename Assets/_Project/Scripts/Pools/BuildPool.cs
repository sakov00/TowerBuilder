using System.Collections.Generic; 
using System.Linq;
using _Project.Scripts.Enums;
using _Project.Scripts.Factories;
using _Project.Scripts.GameObjects;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Pools
{
    public class BuildPool
    {
        [Inject] private BuildFactory _buildFactory;
        
        private Transform _containerTransform;
        private readonly List<BuildController> _availableBuilds = new();

        public void SetContainer(Transform transform)
        {
            _containerTransform = transform;
        }
        
        public List<BuildController> GetAvailableBuilds() => _availableBuilds;
        
        public BuildController Get(BuildType buildType, Transform parent, Vector3 position = default, Quaternion rotation = default) 
        {
            var build = _availableBuilds.FirstOrDefault(c => c.Model.BuildType == buildType);
            if (build != null)
            {
                _availableBuilds.Remove(build);
                build.transform.DOKill();
                build.transform.position = position;
                build.transform.rotation = rotation;
            }
            else
            {
                build = _buildFactory.CreateBuild(buildType, parent, position, rotation);
            }

            build.transform.SetParent(parent);
            return build;
        }

        public void Return(BuildController build)
        {
            if (!_availableBuilds.Contains(build))
            {
                _availableBuilds.Add(build);
            }
            
            build.gameObject.SetActive(false);
            build.transform.SetParent(_containerTransform, false); 
        }
        
        public void Remove(BuildController build)
        {
            if (!_availableBuilds.Contains(build))
            {
                _availableBuilds.Remove(build);
            }
        }
    }
}