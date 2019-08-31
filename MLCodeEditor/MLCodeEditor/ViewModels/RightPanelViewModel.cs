using MLCodeEditor.Messages;
using Prism.Events;
using Prism.Mvvm;

namespace MLCodeEditor.ViewModels
{
    class RightPanelViewModel :BindableBase
    {
        private readonly IEventAggregator _ea;
        private string _fontColorTheme ="White";
        public string FontColorTheme
        {
            get { return _fontColorTheme; }
            set { SetProperty(ref _fontColorTheme, value); }
        }

        private string _ttheme;
        public string Theme
        {
            get { return _ttheme; }
            set
            {
                SetProperty(ref _ttheme, value);

                string mainEditorColor = "#f9f9f9";
                string mainEditorFontColor = "#272727";

                if (string.IsNullOrEmpty(bTheme))
                {
                    mainEditorColor = "#f9f9f9";
                    mainEditorFontColor = "#272727";
                }
                else if (bTheme.Contains("Formal"))
                {
                    mainEditorColor = "#f9f9f9";
                    mainEditorFontColor = "#272727";
                }
                else if (bTheme.Contains("Light"))
                {
                    mainEditorColor = "#ebfffe";
                    mainEditorFontColor = "Black";
                }
                _ea.GetEvent<ThemeMessage>().Publish(mainEditorColor);
                _ea.GetEvent<fontColorThemeMessage>().Publish(mainEditorFontColor);
            }
        } 

        private string _theme;
        public string bTheme
        {
            get { return _theme; }
            set
            {
                SetProperty(ref _theme, value);
                if (string.IsNullOrEmpty(bTheme)) Theme = "#dbdbdb";
                else if (bTheme.Contains("Formal")) Theme = "#dbdbdb";
                else if (bTheme.Contains("Light")) Theme = "#6486de";

            }
        }
      
        private string _font;
        public string bFont
        {
            get { return _font; }
            set { SetProperty(ref _font, value); }
        }

        private string _language;

        public string bLanguage
        {
            get { return _language; }
            set
            {
                SetProperty(ref _language, value);
                string language = "";
                if (string.IsNullOrEmpty(bLanguage)) language = "C++";
                else if (bLanguage.Contains("C++")) language = "C++";
                else if (bLanguage.ToLower().Contains("java")) language = "Java";
                else if (bLanguage.ToLower().Contains("csharp")) language = "C#";
                else if (bLanguage.ToLower().Contains("js")) language = "JavaScript";
                else if (bLanguage.ToLower().Contains("html")) language = "HTML";
                else if (bLanguage.ToLower().Contains("xml")) language = "XML";

                _ea.GetEvent<syntaxThemeMessage>().Publish(language);
            }
        }

        public RightPanelViewModel(IEventAggregator ea )
        {
            this._ea = ea;
        }
    }
}
