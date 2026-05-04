using System.Collections.Generic;
using UnityEngine;

namespace UI.SpecialItems.Paint
{
    public class FingerDrawerGroup : MonoBehaviour
    {
        [Header("Painter Settings")]
        public List<FingerDrawerGPU> targets;
        public List<Color> colors;
        
        public void SetColor(int index)
        {
            targets.ForEach(x=> x.brushColor = colors[index]);
        }
        
        public void SetEnabled(bool enabled)
        {
            targets.ForEach(x=> x.SetEnabled(enabled));
        }
    }
}