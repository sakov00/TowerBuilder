using System;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.Registries;
using _Project.Scripts.Services;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.GameObjects
{
    public class MultiplyAddScoreZone : MonoBehaviour
    {
        [Inject] private MultiplyZonesRegistry _multiplyZonesRegistry;
        [Inject] private SettingsService _settingsService;
        [Inject] private AppData _appData;

        [SerializeField] private ParticleSystem _effect;

        private void Awake()
        {
            _multiplyZonesRegistry.Register(this);
        }

        public void IsCompletedStage(BuildController placedBlock)
        {
            if(placedBlock.transform.position.y <= transform.position.y || enabled == false)
                return;
            
            _effect.Play();
            _settingsService.PlaySfx(SoundKey.HappyConfettiPop);
            _appData.LevelData.AddScoreValue *= 2;
            enabled = false;
        }

        public void Reset()
        {
            _effect.Stop();
            enabled = true;
        }
        
        private void OnDestroy()
        {
            _multiplyZonesRegistry.Unregister(this);
        }
    }
}