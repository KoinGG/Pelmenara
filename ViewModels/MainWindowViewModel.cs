
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
        public ReactiveCommand<Unit, Unit> IsUserAuthorisedChanges { get; }

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
                MessageBoxManager.GetMessageBoxStandardWindow("Время авторизации истекло", "ашибка ашибка", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                authWindow.Close();
            }
            finally
            {
                IsUserAuthorisedChangesImpl(window as MainWindow);
            }
        }

        private void IsUserAuthorisedChangesImpl(MainWindow window)
        {
            if(User.UserId != 0)
            {
                Text00 = "0000";
                Text01 = "0101";
                Text11 = "1111";
            }
            window.Title = $"Pelmenara: {User.Login}";
            window.btn_SignIn.IsEnabled = false;
        }

        public MainWindowViewModel()
        {
            SignInCommand = ReactiveCommand.Create<Window>(SignInCommandImpl);
            //IsUserAuthorisedChanges = ReactiveCommand.Create(IsUserAuthorisedChangesImpl);
        }
    }
}
