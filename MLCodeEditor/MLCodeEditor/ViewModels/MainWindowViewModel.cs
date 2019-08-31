using System;
using System.IO;
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



        public ICSharpCode.AvalonEdit.TextEditor editor { get; set; }

        private async void onSaveFile()
        {
            if(bFileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false) bFileName = dlg.FileName;
            }

            if (bFileName == null) return;
            if (File.Exists(bFileName)) File.Delete(bFileName);

            using (var stream = File.Open(bFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var sr = new StreamWriter(stream))
                {
                    sr.Write(bSourceCode ?? "");
                }
            }
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
            _ea.GetEvent<rightPanelTalkMessage>().Subscribe(onClickToTalk);
        }

        async void onClickToTalk()
        {
            _ea.GetEvent<onAzureMLWorking>().Publish();
            var ret = await _messageListener.RecognizeSpeechSync();
            OperateCommandAsync(ret.result);
            _ea.GetEvent<onAzureMLEnded>().Publish(ret.origin);
        }

        private async void OperateCommandAsync(string result)
        {
            if( result == "save")
            {
                onSaveFile();
            }
            else if ( result == "saveAsExit")
            {
                onSaveFile();
                foreach (System.Windows.Window window in System.Windows.Application.Current.Windows)
                {
                    window.Close();
                }
            }
            else if ( result == "moveBottom")
            {
                editor.ScrollToEnd();
            }
            else if (result == "moveTop")
            {
                editor.ScrollToHome();
            }
            else if (result == "zoomIn")
            {
                editor.FontSize += 5;
                editor.FontSize = Math.Min(editor.FontSize, 60);
            }
            else if (result == "zoomOut")
            {
                editor.FontSize -= 5;
                editor.FontSize = Math.Max(editor.FontSize, 0);
            }
        }

        private void onChangeTheme(string theme) => bTheme = theme;
        private void onChangeFontColorTheme(string fontColor) => bFontColorTheme = fontColor;
        private void onChangeSyntaxTheme(string syntax) => bSyntaxHighlighting = syntax;
    }
}
