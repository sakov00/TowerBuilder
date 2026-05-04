using _Project.Scripts._GlobalLogic;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.AllAppData
{
    public class User
    {
        private readonly IntReactiveProperty _currentLevel = new(0);
        private readonly IntReactiveProperty _crystals = new(15200);
        private readonly BoolReactiveProperty _soundIsActive = new(true);
        private readonly BoolReactiveProperty _musicIsActive = new(true);
        private readonly BoolReactiveProperty _vibroIsActive = new(true);

        public IReactiveProperty<int> CurrentLevelReactive => _currentLevel;
        public IReactiveProperty<int> CrystalsReactive => _crystals;
        public IReactiveProperty<bool> SoundIsActiveReactive => _soundIsActive;
        public IReactiveProperty<bool> MusicIsActiveReactive => _musicIsActive;
        public IReactiveProperty<bool> VibroIsActiveReactive => _vibroIsActive;

        public int CurrentLevel
        {
            get => _currentLevel.Value;
            set
            {
                _currentLevel.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.CurrentLevel, CurrentLevel);
                PlayerPrefs.Save();
            }
        }

        public int Crystals
        {
            get => _crystals.Value;
            set
            {
                _crystals.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.Crystals, Crystals);
                PlayerPrefs.Save();
            }
        }
        
        public bool SoundIsActive
        {
            get => _soundIsActive.Value;
            set
            {
                _soundIsActive.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.SoundIsActive, SoundIsActive ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public bool MusicIsActive
        {
            get => _musicIsActive.Value;
            set
            {
                _musicIsActive.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.MusicIsActive, MusicIsActive ? 1 : 0);
                PlayerPrefs.Save();
            }
        }
        
        public bool VibroIsActive
        {
            get => _vibroIsActive.Value;
            set
            {
                _vibroIsActive.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.VibroIsActive, VibroIsActive ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public User()
        {
            CurrentLevel = PlayerPrefs.GetInt(GameConstants.PrefKeys.CurrentLevel, 0);
            Crystals = PlayerPrefs.GetInt(GameConstants.PrefKeys.Crystals, 0);
            SoundIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.SoundIsActive, 0) == 1;
            MusicIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.MusicIsActive, 0) == 1;
            VibroIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.VibroIsActive, 0) == 1;
        }
    }
}