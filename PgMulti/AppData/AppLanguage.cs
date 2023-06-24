using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PgMulti.AppData
{
    public class AppLanguage
    {
        private const string DefaultLanguageId = "en";

        public readonly string Id;
        public readonly string Name;

        public AppLanguage(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public CultureInfo CultureInfo
        {
            get
            {
                return CultureInfo.GetCultureInfo(Id);
            }
        }

        private static List<AppLanguage>? _AvailableLanguages = null;
        public static List<AppLanguage> AvailableLanguages
        {
            get
            {
                if (_AvailableLanguages == null)
                {
                    _AvailableLanguages = new List<AppLanguage>
                    {
                        new AppLanguage("es", "Español"),
                        new AppLanguage("en", "English")
                    };
                }

                return _AvailableLanguages;
            }
        }

        private static AppLanguage? _CurrentLanguage = null;
        public static AppLanguage CurrentLanguage
        {
            get
            {
                if (_CurrentLanguage == null)
                {
                    if (string.IsNullOrWhiteSpace(Properties.AppSettings.Default.LanguageId))
                    {
                        if (_CurrentLanguage == null)
                        {
                            _CurrentLanguage = AvailableLanguages.FirstOrDefault(lg => lg.Id == Application.CurrentCulture.TwoLetterISOLanguageName);
                        }
                        if (_CurrentLanguage == null)
                        {
                            _CurrentLanguage = AvailableLanguages.First(lg => lg.Id == DefaultLanguageId);
                        }

                        Properties.AppSettings.Default.LanguageId = _CurrentLanguage.Id;
                        Properties.AppSettings.Default.Save();
                    }
                    else 
                    {
                        _CurrentLanguage = AvailableLanguages.First(lg => lg.Id == Properties.AppSettings.Default.LanguageId);
                    }
                }

                return _CurrentLanguage;
            }
            set
            {
                _CurrentLanguage = value;
                Properties.AppSettings.Default.LanguageId = _CurrentLanguage.Id;
                Properties.AppSettings.Default.Save();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
