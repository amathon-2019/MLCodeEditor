using Prism.Unity;
using Microsoft.Practices.Unity;
using System.Windows;
using MLCodeEditor.Views;
using MLCodeEditor.Messages;

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
            //Container.RegisterType<MessageListener, MessageListener>(new InjectionConstructor("ko-KR"));
            Application.Current.MainWindow.Show();
        }
    }
}
