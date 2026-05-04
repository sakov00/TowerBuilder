using _Project.Scripts.GameObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Scripts._GlobalLogic
{
    public class GlobalObjects : MonoBehaviour
    {
        public static GlobalObjects Instance { get; private set; }
        public static EventSystem EventSystem => Instance._eventSystem;
        
        public static Camera Camera => Instance._camera;
        
        [SerializeField] private Camera _camera;
        [SerializeField] private EventSystem _eventSystem;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}