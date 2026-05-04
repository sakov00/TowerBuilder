using _Project.Scripts._GlobalLogic;
using UnityEngine;

namespace _Project.Scripts.UI.Info
{
    public class UniversalBar : MonoBehaviour
    {
        [SerializeField] protected MeshRenderer _universalBarRenderer;
        private MaterialPropertyBlock _matBlock;

        private void OnValidate()
        {
            _universalBarRenderer ??= GetComponent<MeshRenderer>();
        }

        public virtual void Awake() 
        {
            _universalBarRenderer.enabled = false;
            _matBlock = new MaterialPropertyBlock();
        }
        
        public void UpdateBar(float currentValue, float maxValue) 
        {
            if (currentValue <= 0 || currentValue > maxValue)
            {
                _universalBarRenderer.enabled = false;
            }
            else
            {
                _universalBarRenderer.enabled = true;
                BarLookAtCamera();
                ChangeBarValue(currentValue, maxValue);
            } 
        }

        private void ChangeBarValue(float currentValue, float maxValue) 
        {
            _universalBarRenderer.GetPropertyBlock(_matBlock);
            _matBlock.SetFloat("_Fill", currentValue / maxValue);
            _universalBarRenderer.SetPropertyBlock(_matBlock);
        }

        private void BarLookAtCamera() 
        {
            if (GlobalObjects.Camera != null) 
            {
                var camXform = GlobalObjects.Camera.transform;
                var forward = _universalBarRenderer.transform.position - camXform.position;
                forward.Normalize();
                var up = Vector3.Cross(forward, camXform.right);
                _universalBarRenderer.transform.rotation = Quaternion.LookRotation(forward, up);
            }
        }
    }
}