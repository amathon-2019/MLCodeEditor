using System;
using System.IO;
using ICSharpCode.AvalonEdit.Highlighting;
using Microsoft.Win32;
using MLCodeEditor.Messages;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace MLCodeEditor.ViewModels
{
    class MainWindowViewModel : BindableBase
    {

        private string _ttheme;
        public string bTheme
        {
            get { return _ttheme; }
            set { SetProperty(ref _ttheme, value); }
        }

        private string _fontColorTheme;
        public string bFontColorTheme
        {
            get { return _fontColorTheme; }
            set { SetProperty(ref _fontColorTheme, value); }
        }

        private string _syntaxHighlighting;
        public string bSyntaxHighlighting
        {
            get { return _syntaxHighlighting; }
            set { SetProperty(ref _syntaxHighlighting, value); }
        }

        private string _fileName;
        public string bFileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        private string _sourceCode;
        public string bSourceCode
        {
            get { return _sourceCode; }
            set { SetProperty(ref _sourceCode, value); }
        }


        private DelegateCommand _clickToTalk;
        public DelegateCommand cClickToTalk =>
            _clickToTalk ?? (_clickToTalk = new DelegateCommand(onClickToTalk));

        private DelegateCommand _saveFile;
        public DelegateCommand cSaveFile =>
            _saveFile ?? (_saveFile = new DelegateCommand(onSaveFile));


        private DelegateCommand _openFile;
        public DelegateCommand cOpenFile =>
            _openFile ?? (_openFile = new DelegateCommand(onOpenFile));

        private void onOpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if(dlg.ShowDialog() ?? false)
            {
                bFileName = dlg.FileName;
                File.ReadAllText(bFileName);
                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(bFileName));
            }

        }

        private void onSaveFile()
        {
        }

        void oncSaveFile()
        {

        }

        private readonly IEventAggregator _ea;
        private readonly MessageListener _messageListener;

        public MainWindowViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _messageListener = new MessageListener("ko-KR");
            _ea.GetEvent<ThemeMessage>().Subscribe(onChangeTheme);
            _ea.GetEvent<fontColorThemeMessage>().Subscribe(onChangeFontColorTheme);
            _ea.GetEvent<syntaxThemeMessage>().Subscribe(onChangeSyntaxTheme);
        }

        async void onClickToTalk()
        {
            //string result = await _messageListener.RecognizeSpeechSync();
            string result = "저장하기";
            OperateCommandAsync(result);
        }

        private async void OperateCommandAsync(string result)
        {
            if( result == SpeakOrder.저장하기.ToString() )
            {

            }
            else if ( result == SpeakOrder.저장하고나가기.ToString())
            {

            }
            else if ( result == "scroll down")
            {

            }
            else if (result == "scroll up")
            {

            }
            else if (result == "theme dark")
            {

            }
            else if (result == "theme light")
            {

            }
        }

        private void onChangeTheme(string theme) => bTheme = theme;
        private void onChangeFontColorTheme(string fontColor) => bFontColorTheme = fontColor;
        private void onChangeSyntaxTheme(string syntax) => bSyntaxHighlighting = syntax;
    }
}
