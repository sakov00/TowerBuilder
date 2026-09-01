using System;
using System.Collections.Generic;
using UniRx;
using VContainer.Unity;
using YG;

namespace _Project.Scripts.Services
{
    public enum Language
    {
        English,
        Russian,
        Turkish,
        German,
        French,
        Spanish,
        Polish,
        Belarusian,
        Ukrainian,
        Kazakh
    }
    
    public enum TextKey
    {
        Perfect,
        NearMiss
    }

    public class LanguageService : IInitializable, IDisposable
    {
        private readonly ReactiveProperty<Language> _currentLanguage = new(Language.Russian);
        public IReadOnlyReactiveProperty<Language> CurrentLanguage => _currentLanguage;
        
        private readonly Dictionary<TextKey, (string en, string ru)> _texts = new()
        {
            { TextKey.Perfect, ("PERFECT!", "ИДЕАЛЬНО!") },
            { TextKey.NearMiss, ("LUCKY!", "УДАЧНО!") },
        };

        public void Initialize()
        {
            YG2.onGetSDKData += OnGetSDKData;
            YG2.onSwitchLang += SetPlatformLanguage;
        }

        private void OnGetSDKData()
        {
            YG2.GetLanguage();
        }
        
        public void SetPlatformLanguage(string language = "ru")
        {
            if (string.IsNullOrEmpty(language))
                language = "ru";
            
            _currentLanguage.Value = FromCode(language);
        }
        
        public string Get(TextKey key)
        {
            var text = _texts[key];

            return _currentLanguage.Value switch
            {
                Language.English => text.en,
                Language.Russian => text.ru,
                _ => text.ru,
            };
        }

        private static Language FromCode(string code)
        {
            return code switch
            {
                "ru" => Language.Russian,
                "en" => Language.English,
                "tr" => Language.Turkish,
                "de" => Language.German,
                "fr" => Language.French,
                "es" => Language.Spanish,
                "pl" => Language.Polish,
                "be" => Language.Belarusian,
                "uk" => Language.Ukrainian,
                "kk" => Language.Kazakh,
                _ => Language.Russian
            };
        }

        public void Dispose()
        {
            YG2.onGetSDKData -= OnGetSDKData;
            YG2.onSwitchLang -= SetPlatformLanguage;
        }
    }
}