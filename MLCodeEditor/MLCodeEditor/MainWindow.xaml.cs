using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml;

using Microsoft.Win32;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Search;

namespace MLCodeEditor
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

            SearchPanel.Install(textEditor);
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if(dlg.ShowDialog() ?? false)
            {
                currentFileName = dlg.FileName;
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
                    currentFileName = dlg.FileName;
                else
                    return;
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
    }
}
