using System;
using _Project.Scripts._VContainer;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Services
{
    [RequireComponent(typeof(TMP_Text))]
    public class LanguageText : MonoBehaviour
    {
        [Inject] private LanguageService _languageService;

        [SerializeField] private TMP_Text _text;
        [SerializeField] private Translation _translation;

        [Serializable]
        public struct Translation
        {
            [TextArea]
            public string English;

            [TextArea]
            public string Russian;

            public string Get(Language language)
            {
                return language switch
                {
                    Language.Russian => Russian,
                    Language.English => English,
                    _ => Russian
                };
            }
        }
        
        private CompositeDisposable _disposables;

        private void OnValidate()
        {
            _text ??= gameObject.GetComponent<TMP_Text>();
        }

        private void Awake()
        {
            InjectManager.Inject(this);
        }

        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            _languageService.CurrentLanguage
                .Subscribe(UpdateText)
                .AddTo(_disposables);
        }

        private void OnDisable() => _disposables.Clear();
        private void OnDestroy() => _disposables.Clear();

        private void UpdateText(Language language)
        {
            _text.text = _translation.Get(language);
        }
    }
}