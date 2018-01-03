using Prism.Mvvm;
using System.Windows;
using ToDoManager_MUI.Common;
using ToDoManager_MUI.Views;
using Unity;

namespace ToDoManager_MUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // アプリで管理するコンテナ
        private IUnityContainer Container { get; } = new UnityContainer();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => this.Container.Resolve(x));
            this.Container.Resolve<MainWindowView>().Show();
        }
    }
}
