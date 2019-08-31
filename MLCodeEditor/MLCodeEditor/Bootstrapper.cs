using Prism.Unity;
using Microsoft.Practices.Unity;
using System.Windows;
using MLCodeEditor.Views;

namespace MLCodeEditor
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
