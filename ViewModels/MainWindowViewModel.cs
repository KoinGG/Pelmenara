
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Pelmenara_AUI_RUI.Views;
using Pelmenara_AUI_RUI.Sourses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string? _text00 = "00";
        private string? _text01 = "01";
        private string? _text11 = "11";
        public static User User { get; set; } = new User();
        public ReactiveCommand<Window, Unit> SignInCommand { get; }
        public ReactiveCommand<Window, Unit> SignOutCommand { get; }
        public ReactiveCommand<Unit, Unit> IsUserAuthorisedChanges { get; }
        //public ReactiveCommand<Unit, Unit> Abort { get; }

        public string? Text00
        {
            get => _text00;
            set
            {
                this.RaiseAndSetIfChanged(ref _text00, value);
            }
        }
        public string? Text01
        {
            get => _text01;
            set
            {
                this.RaiseAndSetIfChanged(ref _text01, value);
            }
        }
        public string? Text11
        {
            get => _text11;
            set
            {
                this.RaiseAndSetIfChanged(ref _text11, value);
            }
        }


        private async void SignInCommandImpl(Window window)
        {
            AuthWindow authWindow = new AuthWindow();
            try
            {
                await authWindow.ShowDialog(window).WaitAsync(TimeSpan.FromMinutes(60));
            }
            catch
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Время авторизации истекло", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                authWindow.Close();
            }
            finally
            {
                IfUserAuthorisedChangesImpl(window as MainWindow);
            }
        }

        private void SignOutCommandImpl(Window window)
        {
            if (User.UserId != 0)
            {
                User.UserId = -1;
                Text00 = "00";
                Text01 = "01";
                Text11 = "11";
                (window as MainWindow).btn_SignIn.IsEnabled = true;
                (window as MainWindow).btn_SignIn.IsVisible = true;
                (window as MainWindow).btn_SignOut.IsEnabled = false;
                (window as MainWindow).btn_SignOut.IsVisible = false;
            }
            
        }
        private void IfUserAuthorisedChangesImpl(MainWindow window)
        {
            if(User.UserId != 0)
            {
                Text00 = "0000";
                Text01 = "0101";
                Text11 = "1111";
                window.Title = $"Pelmenara: {User.Login}";
                window.btn_SignIn.IsEnabled = false;
                window.btn_SignIn.IsVisible = false;
                window.btn_SignOut.IsEnabled = true;
                window.btn_SignOut.IsVisible = true;
            }
        }

        public MainWindowViewModel()
        {
            //Helper.GetContext();
            SignInCommand = ReactiveCommand.Create<Window>(SignInCommandImpl);
            SignOutCommand = ReactiveCommand.Create<Window>(SignOutCommandImpl);
        }
    }
}
