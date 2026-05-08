using _Project.Scripts._GlobalLogic;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.AllAppData
{
    public class User
    {
        private readonly IntReactiveProperty _crystals = new(15200);
        private readonly BoolReactiveProperty _soundIsActive = new(true);
        private readonly BoolReactiveProperty _musicIsActive = new(true);
        private readonly BoolReactiveProperty _vibroIsActive = new(true);

        public IReactiveProperty<int> CrystalsReactive => _crystals;
        public IReactiveProperty<bool> SoundIsActiveReactive => _soundIsActive;
        public IReactiveProperty<bool> MusicIsActiveReactive => _musicIsActive;
        public IReactiveProperty<bool> VibroIsActiveReactive => _vibroIsActive;

        public int Crystals
        {
            get => _crystals.Value;
            set
            {
                _crystals.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.Crystals, Crystals);
            }
        }
        
        public bool SoundIsActive
        {
            get => _soundIsActive.Value;
            set
            {
                _soundIsActive.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.SoundIsActive, SoundIsActive ? 1 : 0);
            }
        }

        public bool MusicIsActive
        {
            get => _musicIsActive.Value;
            set
            {
                _musicIsActive.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.MusicIsActive, MusicIsActive ? 1 : 0);
            }
        }
        
        public bool VibroIsActive
        {
            get => _vibroIsActive.Value;
            set
            {
                _vibroIsActive.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.VibroIsActive, VibroIsActive ? 1 : 0);
            }
        }

        public User()
        {
            Crystals = PlayerPrefs.GetInt(GameConstants.PrefKeys.Crystals, 0);
            SoundIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.SoundIsActive, 0) == 1;
            MusicIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.MusicIsActive, 0) == 1;
            VibroIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.VibroIsActive, 0) == 1;
        }
    }
}