using System.Collections.Generic;
using UnityEngine;

namespace UI.SpecialItems
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> _particles;
        [SerializeField] private List<Color> _colors;

        public void SetColor(int index)
        {
            if (index < 0 || index >= _colors.Count)
            {
                Debug.LogWarning($"[ParticleController] Invalid color index: {index}");
                return;
            }

            Color color = _colors[index];

            foreach (var particle in _particles)
            {
                if (particle == null)
                    continue;

                var main = particle.main;
                main.startColor = color;
            }
        }

        public void RemoveParticle(ParticleSystem particle)
        {
            _particles.Remove(particle);
        }
    }
}