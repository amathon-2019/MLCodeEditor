using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;
using MLCodeEditor.ViewModels;

namespace MLCodeEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentFileName;
        private const String ProgramTitle = "MLCodeEditor";

        public MainWindow()
        {
            InitializeComponent();
            textEditor.TextArea.Caret.PositionChanged += DisplayPosition;
            textEditor.PreviewMouseWheel += (s, e) =>
            {
                if (!Keyboard.IsKeyDown(Key.LeftCtrl))
                    return;
                if (e.Delta > 0)
                    textEditor.FontSize += 3;
                else if (e.Delta < 0 && textEditor.FontSize > 3)
                    textEditor.FontSize -= 3;
            };

            SearchPanel.Install(textEditor);
            var myvm = this.DataContext as MainWindowViewModel;
            myvm.editor = textEditor;
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if(dlg.ShowDialog() ?? false)
            {
                var myvm = this.DataContext as MainWindowViewModel;
                myvm.bFileName = currentFileName = dlg.FileName;
                textEditor.Load(currentFileName);
                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
            }
            this.Title = currentFileName + " - " + ProgramTitle;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(currentFileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false)
                {
                    var myvm = this.DataContext as MainWindowViewModel;
                    myvm.bFileName = currentFileName = dlg.FileName;
                }
                else return;
            }
            textEditor.Save(currentFileName);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void DisplayPosition(object sender, EventArgs e)
        {
            TextViewPosition tp = textEditor.TextArea.Caret.Position;
            lblPos.Text = "Line: " + tp.Line + " Col: " + tp.Column;
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            var myvm = this.DataContext as MainWindowViewModel;
            myvm.bSourceCode = textEditor.Document.Text;
        }
    }
}
