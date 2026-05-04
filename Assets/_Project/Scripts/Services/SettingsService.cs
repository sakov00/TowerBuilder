using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.AllAppData;
using _Project.Scripts.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.Scripts.Services
{
    public class SettingsService : MonoBehaviour, IInitializable
    {
        [Inject] private AppData _appData;
        
        [System.Serializable]
        public class Sound
        {
            public SoundKey key;
            public AudioClip clip;
        }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Sounds")]
        [SerializeField] private List<Sound> _musicClips = new();
        [SerializeField] private List<Sound> _sfxClips = new();

        private readonly Dictionary<SoundKey, AudioClip> _music = new();
        private readonly Dictionary<SoundKey, AudioClip> _sfx = new();

        private CancellationTokenSource _fadeCts;
        private float _targetMusicVolume = 1f;

        public void Initialize()
        {
            foreach (var s in _musicClips)
                _music[s.key] = s.clip;

            foreach (var s in _sfxClips)
                _sfx[s.key] = s.clip;

            _musicSource.volume = 0f;
        }
        
        public async UniTask PlayMusicAsync(SoundKey key, float fadeDuration = 1f, bool loop = true)
        {
            if (!_music.TryGetValue(key, out var newClip))
            {
                Debug.LogWarning($"Музыка '{key}' не найдена.");
                return;
            }

            if (_musicSource.clip == newClip && _musicSource.isPlaying)
                return;

            CancelFade();

            _fadeCts = new CancellationTokenSource();
            var token = _fadeCts.Token;

            float previousVolume = _musicSource.volume;

            if (_musicSource.isPlaying)
            {
                await FadeVolumeAsync(_musicSource, previousVolume, 0f, fadeDuration * 0.5f, token);
                _musicSource.Stop();
            }
            
            _musicSource.clip = newClip;
            _musicSource.loop = loop;
            _musicSource.volume = 0f;
            _musicSource.Play();

            await FadeVolumeAsync(_musicSource, 0f, _targetMusicVolume, fadeDuration * 0.5f, token);
        }

        public void PlaySfx(SoundKey key)
        {
            if (_sfx.TryGetValue(key, out var clip))
                _sfxSource.PlayOneShot(clip);
            else
                Debug.LogWarning($"SFX '{key}' не найден.");
        }

        private async UniTask FadeVolumeAsync(AudioSource source, float start, float end, float duration, CancellationToken token)
        {
            if (duration <= 0f)
            {
                source.volume = end;
                return;
            }

            float startTime = Time.time;
            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                float t = (Time.time - startTime) / duration;
                if (t >= 1f)
                    break;

                source.volume = Mathf.Lerp(start, end, t);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            source.volume = end;
        }

        private void CancelFade()
        {
            if (_fadeCts != null)
            {
                _fadeCts.Cancel();
                _fadeCts.Dispose();
                _fadeCts = null;
            }
        }
    }
}
