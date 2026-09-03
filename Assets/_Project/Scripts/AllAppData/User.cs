using _Project.Scripts._GlobalLogic;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.AllAppData
{
    public class User
    {
        private readonly IntReactiveProperty _crystals = new(15200);
        private readonly IntReactiveProperty _scoreRecord = new(0);
        private readonly BoolReactiveProperty _isTutorialFirstBlockPassed = new(false);
        private readonly BoolReactiveProperty _isTutorialBlocksPassed = new(false);
        private readonly BoolReactiveProperty _isTutorialBalancePassed = new(false);
        private readonly BoolReactiveProperty _soundIsActive = new(true);
        private readonly BoolReactiveProperty _musicIsActive = new(true);
        private readonly BoolReactiveProperty _vibroIsActive = new(true);

        public IReactiveProperty<int> CrystalsReactive => _crystals;
        public IReactiveProperty<int> ScoreRecordReactive => _scoreRecord;
        public IReactiveProperty<bool> IsTutorialFirstBlockPassedReactive => _isTutorialFirstBlockPassed;
        public IReactiveProperty<bool> IsTutorialBlocksPassedReactive => _isTutorialBlocksPassed;
        public IReactiveProperty<bool> IsTutorialBalancePassedReactive => _isTutorialBalancePassed;
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
                PlayerPrefs.Save();
            }
        }
        
        public int ScoreRecord
        {
            get => _scoreRecord.Value;
            set
            {
                if (_scoreRecord.Value >= value)
                    return;
                
                _scoreRecord.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.ScoreRecord, ScoreRecord);
                PlayerPrefs.Save();
            }
        }

        public bool IsTutorialFirstBlockPassed
        {
            get => _isTutorialFirstBlockPassed.Value;
            set
            {
                if(_isTutorialFirstBlockPassed.Value)
                    return;
                
                _isTutorialFirstBlockPassed.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.IsTutorialFirstBlockPassed, IsTutorialFirstBlockPassed ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public bool IsTutorialBlocksPassed
        {
            get => _isTutorialBlocksPassed.Value;
            set
            {
                if(_isTutorialBlocksPassed.Value)
                    return;
                
                _isTutorialBlocksPassed.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.IsTutorialBlocksPassed, IsTutorialBlocksPassed ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        public bool IsTutorialBalancePassed
        {
            get => _isTutorialBalancePassed.Value;
            set
            {
                if(_isTutorialBalancePassed.Value)
                    return;
                
                _isTutorialBalancePassed.Value = value;
                PlayerPrefs.SetInt(GameConstants.PrefKeys.IsTutorialBalancePassed, IsTutorialBalancePassed ? 1 : 0);
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
            Crystals = PlayerPrefs.GetInt(GameConstants.PrefKeys.Crystals, 0);
            ScoreRecord = PlayerPrefs.GetInt(GameConstants.PrefKeys.ScoreRecord, 0);
            
            IsTutorialFirstBlockPassed = PlayerPrefs.GetInt(GameConstants.PrefKeys.IsTutorialFirstBlockPassed, 0) == 1;
            IsTutorialBlocksPassed = PlayerPrefs.GetInt(GameConstants.PrefKeys.IsTutorialBlocksPassed, 0) == 1;
            IsTutorialBalancePassed = PlayerPrefs.GetInt(GameConstants.PrefKeys.IsTutorialBalancePassed, 0) == 1;
            
            SoundIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.SoundIsActive, 1) == 1;
            MusicIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.MusicIsActive, 1) == 1;
            VibroIsActive = PlayerPrefs.GetInt(GameConstants.PrefKeys.VibroIsActive, 1) == 1;
        }
    }
}