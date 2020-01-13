using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using modern_tech_499m.Helpers;
using modern_tech_499m.ViewModels.Base;
using PropertyChanged;

namespace modern_tech_499m.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Internal classes

        public class ItemOption : BaseViewModel
        {
            public string InnerName { get; set; }

            public string DisplayedResourceName { get; set; }

            public string DisplayedName { get; set; }

            public void Update()
            {
                DisplayedName = DisplayedResourceName.GetAsResource<string>();
            }
        }

        #endregion

        #region Private properties

        private List<ItemOption> _languageOptions;
        private ItemOption _selectedLanguage;

        #endregion

        #region Public properties

        [DoNotNotify]
        public List<ItemOption> LanguageOptions
        {
            get => _languageOptions;
            set
            {
                _languageOptions = value;
                OnPropertyChanged();
            }
        }
        [DoNotNotify]
        public ItemOption SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                var oldValue = _selectedLanguage;
                var newValue = value;
                _selectedLanguage = value;
                ChangeLanguage(oldValue, newValue);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public SettingsViewModel()
        {
            LanguageOptions = new List<ItemOption>
            {
                new ItemOption
                {
                    InnerName = "EnglishStrings",
                    DisplayedResourceName = "EnglishLanguageName",
                    DisplayedName = "EnglishLanguageName".GetAsResource<string>(),
                },
                new ItemOption
                {
                    InnerName = "RussianStrings",
                    DisplayedResourceName = "RussianLanguageName",
                    DisplayedName = "RussianLanguageName".GetAsResource<string>()
                }
            };

            _selectedLanguage = LanguageOptions[0];
        }

        #endregion

        #region Private methods

        private void ChangeLanguage(ItemOption oldValue, ItemOption newValue)
        {
            if (oldValue != null && newValue != null && oldValue.InnerName.Equals(newValue.InnerName))
                return;

            //Remove old resource
            if (oldValue != null)
                Application.Current.Resources.MergedDictionaries.Remove(
                    Application.Current.Resources.MergedDictionaries.Single(item =>
                        item.Source.ToString().Contains(oldValue.InnerName)));

            //Add new Value
            if (newValue != null && !Application.Current.Resources.MergedDictionaries.Any(item => item.Source.ToString().Contains(newValue.InnerName)))
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
                { Source = new Uri($"pack://application:,,,/Styles/Localization/{newValue.InnerName}.xaml") });

            UpdateChangedItems();
        }

        private void UpdateChangedItems()
        {
            foreach (var languageOption in LanguageOptions)
                languageOption.Update();
        }

        #endregion
    }
}
