using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts._VContainer
{
    public class InjectManager
    {
        private static IObjectResolver objectResolver;
        
        public static void Initialize(IObjectResolver objectResolverParam)
        {
            objectResolver = objectResolverParam;
        }

        public static void Inject(object target)
        {
            if (objectResolver == null)
            {
                Debug.LogError("InjectManager is not initialized.");
                return;
            }

            objectResolver.Inject(target);
        }

        public static void InjectGameObject(GameObject gameObject)
        {
            if (objectResolver == null)
            {
                Debug.LogError("InjectManager is not initialized.");
                return;
            }

            objectResolver.InjectGameObject(gameObject);
        }
    }
}