using UnityEngine;

namespace _Project.Scripts.GameObjects
{
    public class BlocksContainer : MonoBehaviour
    {
        public void Reset()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}