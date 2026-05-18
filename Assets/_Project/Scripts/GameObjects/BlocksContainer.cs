using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    public class BlocksContainer : MonoBehaviour
    {
        public void Reset()
        {
            if (this == null) return;
            transform.rotation = Quaternion.identity;
        }
    }
}