using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Pelmenara_AUI_RUI.ViewModels;
using Pelmenara_AUI_RUI.Views;

namespace Pelmenara_AUI_RUI
{
    public partial class App : Application
    {
        public string dbpass = "youtubesuperlol";
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //desktop.MainWindow = new MainWindow
                desktop.MainWindow = new MainWindow
                {
                    //DataContext = new MainWindowViewModel(),
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
