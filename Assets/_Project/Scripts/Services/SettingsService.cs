using System;
using _Project.Scripts._VContainer;
using DG.Tweening;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using _Project.Scripts.SO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Services
{
    public class SettingsService : MonoBehaviour
    {
        [Inject] private SoundConfig _soundConfig;
        [Inject] private AppData _appData;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Music Fade")]
        [SerializeField] private float _fadeDuration = 1f;

        private Tween _musicTween;

        private void Awake()
        {
            InjectManager.Inject(this);
        }

        public async UniTaskVoid PlayMusicAsync(SoundKey key, bool loop = true)
        {
            if (_appData.User.MusicIsActive == false)
                return;

            var music = _soundConfig.MusicClips.Find(x => x.key == key);

            if (music == null)
            {
                Debug.LogWarning($"Музыка '{key}' не найдена.");
                return;
            }

            if (_musicSource.clip == music.clip &&
                _musicSource.isPlaying)
                return;

            _musicTween?.Kill();

            // Fade Out
            if (_musicSource.isPlaying)
            {
                await _musicSource
                    .DOFade(0f, _fadeDuration)
                    .SetEase(Ease.Linear)
                    .AsyncWaitForCompletion();
            }

            _musicSource.Stop();

            _musicSource.clip = music.clip;
            _musicSource.loop = loop;
            _musicSource.volume = 0f;

            _musicSource.Play();

            // Fade In
            _musicTween = _musicSource
                .DOFade(music.volume, _fadeDuration)
                .SetEase(Ease.Linear);

            await _musicTween.AsyncWaitForCompletion();
        }

        public void StopMusic()
        {
            _musicTween?.Kill();
            _musicSource.Stop();
        }

        public void PlaySfx(SoundKey key)
        {
            if (_appData.User.SoundIsActive == false)
                return;

            var sfx = _soundConfig.SfxClips.Find(x => x.key == key);

            if (sfx != null)
                _sfxSource.PlayOneShot(sfx.clip, sfx.volume);
            else
                Debug.LogWarning($"SFX '{key}' не найден.");
        }

        public void PlayVibrationPop()
        {
            if (_appData.User.VibroIsActive == false)
                return;

            Vibration.VibratePop();
        }
    }
}